using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Application.Features.Students;
using Testimonial.Application.Features.Teachers;
using Testimonial.Application.Features.Users;
using Testimonial.Domain.Authentication;
namespace Testimonial.Application.Features.Testimonials;

public class CreateTestimonialPdfUseCase : ICreateTestimonialPdfUseCase
{
    private readonly IStudentRepository _students;
    private readonly ITeacherRepository _teachers;
    private readonly ITestimonialRepository _testimonials;
    private readonly ICurrentUserService _currentUser;
    private readonly ITestimonialPdfGenerator _pdf;

    public CreateTestimonialPdfUseCase(IStudentRepository students, ITestimonialPdfGenerator pdf, ITestimonialRepository testimonials, ITeacherRepository teachers, ICurrentUserService currentUser)
    {
        _students = students;
        _teachers = teachers;
        _testimonials = testimonials;
        _currentUser = currentUser;
        _pdf = pdf;
    }

    public async Task<byte[]> ExecuteAsync(int studentId, TestimonialDto dto)
    {
        if (_currentUser.Role != RoleNames.Admin &&
            _currentUser.Role != RoleNames.ClassTeacher)
        {
            throw new UnauthorizedAccessException("Du har ikke rettigheder til at oprette vidnesbyrd.");
        }

        var student = await _students.GetByStudentIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException($"Student '{studentId}' not found.");

        dto.StudentFullName = $"{student.FirstName} {student.LastName}";
        dto.ClassName = student.Class?.Name ?? "Ukendt klasse";

        var testimonial = MapToDomain(studentId, dto);

        //Gemmes i DB
        await _testimonials.AddAsync(testimonial);
        await _testimonials.SaveChangesAsync();



        //Generate PDF from dto
        return _pdf.Generate(dto);
    }

    private static Domain.Entities.Testimonial MapToDomain(int studentId, TestimonialDto dto)
    {
        return new Domain.Entities.Testimonial
        {
            Title = "",
            StudentId = studentId,
            SchoolYear = dto.Year,
            DateCreated = DateOnly.FromDateTime(DateTime.UtcNow),
            Grade = dto.ClassName,

            SubjectEntries = dto.Subjects
                .SelectMany(s => s.DescriptionBlocks)
                .Select(b => new Domain.Entities.SubjectEntry
                {
                    SubjectDescription = $"{b.Heading}\n{b.Body}"
                }).ToList()
        };
    }
}