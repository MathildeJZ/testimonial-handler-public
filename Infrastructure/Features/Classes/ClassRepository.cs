using Microsoft.EntityFrameworkCore;
using Testimonial.Application.Features.Classes;
using Testimonial.Domain.Entities;
using Testimonial.Infrastructure.Persistence;
using Testimonial.Infrastructure.Features;


namespace Testimonial.Infrastructure.Features.Classes;

public class ClassRepository : IClassRepository
{
    private readonly AppDbContext _db;

    public ClassRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Class?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Classes
            .FirstOrDefaultAsync(c => c.ClassId == id, ct);
    }

    public async Task<IEnumerable<Class>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Classes.ToListAsync(ct);
    }
}
