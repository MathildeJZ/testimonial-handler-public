using Testimonial.Application.Features.Students;
using Testimonial.Domain.Entities;
using Testimonial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Testimonial.Infrastructure.Features.Students;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _db;

    public StudentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Student?> GetByStudentIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Students
            .FirstOrDefaultAsync(s => s.StudentId == id, ct);
    }

    public async Task<IEnumerable<Student>> GetAllWithClassAsync(CancellationToken ct = default)
    {
        return await _db.Students
            .Include(s => s.Class)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Students.ToListAsync(ct);
    }

    public async Task<Student> CreateAsync(Student student, CancellationToken ct = default)
    {
        _db.Students.Add(student);
        await _db.SaveChangesAsync(ct);
        return student;
    }

    public async Task<Student> UpdateAsync(Student student, CancellationToken ct = default)
    {
        _db.Students.Update(student);
        await _db.SaveChangesAsync(ct);
        return student;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var student = await _db.Students.FindAsync(new object[] { id }, ct);
        if (student == null)
            return false;

        _db.Students.Remove(student);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<Student?> GetByStudentIdWithClassAsync(int id, CancellationToken ct = default)
    {
        return await _db.Students
            .Include(s => s.Class)
            .FirstOrDefaultAsync(s => s.StudentId == id, ct);
    }
}

