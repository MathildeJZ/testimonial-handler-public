using Testimonial.Domain.Entities;

namespace Testimonial.Application.Features.Teachers;

public interface ITeacherRepository
{
    Task<Teacher?> GetByUserId(string userId);
    Task AddTeacherAsync(Teacher teacher);
    Task UpdateTeacherAsync(Teacher teacher);
    Task<Teacher?> GetTeacherByIdAsync(int id);
    Task<List<Teacher>> GetAllTeachersAsync();
}