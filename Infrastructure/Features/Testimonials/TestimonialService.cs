using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Application.Features.Testimonials;
using Testimonial.Infrastructure.Persistence;
using Testimonial.Application.Features;
using Testimonial.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Testimonial.Infrastructure.Features.Testimonials;

public class TestimonialService : ITestimonialService
{
    private readonly ITestimonialRepository _repository;
    private readonly AppDbContext _context;

    public TestimonialService(ITestimonialRepository repository, AppDbContext dbContext)
    {
        _repository = repository;
        _context = dbContext;
    }

    public async Task<IEnumerable<SearchTestimonialResponse>> SearchTestimonialsAsync(SearchTestimonialRequest request)
    {
        var result = await _repository.SearchAsync(request);

        var list = new List<SearchTestimonialResponse>
        {
            result
        };

        return new List<SearchTestimonialResponse> { result };
    }


    public async Task<Domain.Entities.Testimonial> CreateTestimonialAsync(CreateTestimonialRequest request)
    {
        var testimonial = new Domain.Entities.Testimonial
        {
            StudentId = request.StudentId,
            Title = request.Title,
            Grade = request.Grade,
            SchoolYear = request.SchoolYear,

            OverallStudentDescription = request.OverallStudentDescription,
            EndMessage = request.EndMessage,
            DateCreated = DateOnly.FromDateTime(DateTime.Now),

            SubjectEntries = request.Subjects.Select(s =>
            {
                Console.WriteLine($"🔥 DTO TeacherId: {s.TeacherId}");

                var entry = new SubjectEntry
                {
                    SubjectId = s.SubjectId,
                    SubjectDescription = s.SubjectDescription,
                    TeacherId = s.TeacherId
                };

                Console.WriteLine($"🔥 Entity TeacherId: {entry.TeacherId}");

                return entry;
            }).ToList()
        };

        await _repository.AddAsync(testimonial);
        await _context.SaveChangesAsync();

        return testimonial;
    }

    public Task<IEnumerable<Domain.Entities.Testimonial?>> GetTestimonialByIdAsync(string id)
    {
        if (!int.TryParse(id, out var intId))
            return Task.FromResult<IEnumerable<Domain.Entities.Testimonial?>>(Array.Empty<Domain.Entities.Testimonial?>());

        var result = _repository.GetById(intId);

        IEnumerable<Domain.Entities.Testimonial?> list =
            result is null
                ? Array.Empty<Domain.Entities.Testimonial?>()
                : new[] { result };

        return Task.FromResult(list);
    }

    public async Task<List<TestimonialDto>> GetByStudentId(int studentId)
    {
        return await _context.Testimonials
        .Where(t => t.StudentId == studentId)
        .Select(t => new TestimonialDto
        {
            StudentFullName = t.Student.FirstName + " " + t.Student.LastName,
            OverallStudentDescription = t.OverallStudentDescription
        })
        .ToListAsync();
    }

    public Task<bool> DeleteTestimonialAsync(string id)
    {
        if (!int.TryParse(id, out var intId))
            return Task.FromResult(false);

        var result = _repository.Delete(intId);

        return Task.FromResult(result);
    }

    public async Task<Domain.Entities.Testimonial?> GetTestimonialsAsync(int studentId)
    {
        return await _repository.GetByStudentId(studentId);
    }
}

