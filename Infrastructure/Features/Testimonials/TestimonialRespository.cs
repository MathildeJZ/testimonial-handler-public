using Microsoft.EntityFrameworkCore;
using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Application.Features.Testimonials;
using Testimonial.Infrastructure.Persistence;

namespace Testimonial.Infrastructure.Features.Testimonials;

public class TestimonialRepository : ITestimonialRepository
{
    private readonly AppDbContext _db;

    public TestimonialRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Domain.Entities.Testimonial testimonial)
    {
        _db.Testimonials.Add(testimonial);
        await _db.SaveChangesAsync();
    }

    public async Task<Domain.Entities.Testimonial?> GetByStudentId(int studentId, CancellationToken ct = default)
    {
        return await _db.Testimonials
                .Include(t => t.Student)
                .Include(t => t.SubjectEntries)
                    .ThenInclude(se => se.Subject)
                .Include(t => t.SubjectEntries)
                    .ThenInclude(se => se.Teacher)
                .FirstOrDefaultAsync(t => t.StudentId == studentId, ct);
    }

    public Domain.Entities.Testimonial? GetById(int id)
    {
        throw new NotImplementedException();
    }

    // public Domain.Entities.Testimonial Update(Domain.Entities.Testimonial testimonial)
    // {
    //     _db.Testimonials.Update(testimonial);
    //     _db.SaveChanges();

    //     return testimonial;
    // }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<SearchTestimonialResponse> SearchAsync(SearchTestimonialRequest request)
    {
        throw new NotImplementedException();
    }
}


