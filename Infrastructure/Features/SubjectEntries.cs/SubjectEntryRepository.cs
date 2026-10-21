using Testimonial.Application.Features.SubjectEntries;
using Testimonial.Domain.Entities;
using Testimonial.Infrastructure.Persistence;

namespace Testimonial.Infrastructure.Features.SubjectEntries;

public class SubjectEntryRepository : ISubjectEntryRepository
{
    private readonly AppDbContext _context;

    public SubjectEntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SubjectEntry entry)
    {
        await _context.SubjectEntries.AddAsync(entry);
    }
}