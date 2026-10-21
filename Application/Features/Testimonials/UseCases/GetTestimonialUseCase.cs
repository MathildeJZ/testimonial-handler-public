using Testimonial.Application.Features.Teachers;
using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Application.Features.Users;
using Testimonial.Domain.Authentication;
using Testimonial.Domain.Entities;
using System.Linq;

namespace Testimonial.Application.Features.Testimonials.UseCases;

public class GetTestimonialUseCase : IGetTestimonialUseCase
{
    private readonly ITestimonialRepository _testimonials;
    private readonly ICurrentUserService _currentUser;
    private readonly ITeacherRepository _teachers;

    public GetTestimonialUseCase(
        ITestimonialRepository testimonials,
        ICurrentUserService currentUser,
        ITeacherRepository teachers)
    {
        _testimonials = testimonials;
        _currentUser = currentUser;
        _teachers = teachers;
    }

    public async Task<TestimonialDto> ExecuteAsync(int studentId)
    {
        // Hent testimonial
        var testimonial = await _testimonials.GetByStudentId(studentId);
        if (testimonial == null)
            throw new KeyNotFoundException($"No testimonial found for '{studentId}'");

        // Hent læreren én gang
        var teacher = await _teachers.GetByUserId(_currentUser.UserId);
        if (teacher == null)
            throw new KeyNotFoundException("Teacher not found for current user");

        // Hvis brugeren er subject teacher → filtrér entries
        if (_currentUser.Role == RoleNames.SubjectTeacher)
        {
            testimonial.SubjectEntries = testimonial.SubjectEntries
                .Where(entry => entry.TeacherId == teacher.TeacherId)
                .ToList();
        }

        // Returnér DTO
        return MapToDto(testimonial);
    }

    private static TestimonialDto MapToDto(Testimonial.Domain.Entities.Testimonial testimonial)
    {
        return new TestimonialDto
        {
            Subjects = testimonial.SubjectEntries
                .GroupBy(e => e.SubjectId)
                .Select(group =>
                {
                    var statementEntry = group.Last();

                    return new SubjectSectionDto
                    {
                        SubjectName = statementEntry.Subject.Name,
                        TeacherStatement = statementEntry.PersonalStatement,
                        TeacherName = statementEntry.Teacher.FirstName + " " + statementEntry.Teacher.LastName,
                        DescriptionBlocks = group
                            .OrderBy(e => e.OrderIndex)
                            .Select(entry => new DescriptionBlockDto
                            {
                                Heading = entry.Subject.Name,
                                Body = entry.SubjectDescription
                            })
                            .ToList()
                    };
                })
                .ToList()
        };
    }
}
