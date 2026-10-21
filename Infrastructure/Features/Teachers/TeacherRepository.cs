using Microsoft.EntityFrameworkCore;
using Testimonial.Application.Features.Teachers;
using Testimonial.Domain.Entities;
using Testimonial.Infrastructure.Persistence;

namespace Testimonial.Infrastructure.Features.Teachers;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;

    public TeacherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher?> GetByUserId(string userId)
    {
        return await _context.Teacher
            .Include(t => t.SubjectEntries)
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task AddTeacherAsync(Teacher teacher)
    {
        _context.Teacher.Add(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTeacherAsync(Teacher teacher)
    {
        _context.Teacher.Update(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task<Teacher?> GetTeacherByIdAsync(int id)
    {
        return await _context.Teacher
            .Include(t => t.SubjectEntries)
            .FirstOrDefaultAsync(t => t.TeacherId == id);
    }

    public async Task<List<Teacher>> GetAllTeachersAsync()
    {
        return await _context.Teacher
            .Include(t => t.SubjectEntries)
            .ToListAsync();
    }
}

