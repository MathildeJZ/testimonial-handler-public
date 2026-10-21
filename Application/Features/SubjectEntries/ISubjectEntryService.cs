using Testimonial.Application.Features.Testimonials;
using Testimonial.Domain.Entities;

namespace Testimonial.Application.Features.SubjectEntries;

public interface ISubjectEntryService
{
    Task<SubjectEntry> CreateSubjetEntryAsync(CreateSubjectEntryRequest request);
}
