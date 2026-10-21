using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Domain.Entities;

namespace Testimonial.Application.UseCases;

public class CreateTestimonialUseCase
{
    private readonly ITestimonialRepository _repository;

    public CreateTestimonialUseCase(ITestimonialRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Testimonial> Execute(
        string title,
        string author,
        int schoolYear,
        List<SubjectEntry> subjectEntries,
        int studentId)
    {
        var newTestimonial = new Domain.Entities.Testimonial
        {
            Title = title,
            SchoolYear = schoolYear,
            StudentId = studentId,
            SubjectEntries = subjectEntries,
            DateCreated = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        return newTestimonial;
    }
}
