using Testimonial.Domain.Entities;

namespace Testimonial.Application.Features.SubjectEntries;

public interface ISubjectEntryRepository
{
    Task AddAsync(SubjectEntry entry);
}