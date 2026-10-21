using Application.Features.Students;
using Testimonial.Domain.Entities;

namespace Testimonial.Application.Features.Students;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<StudentDTO?> GetByStudentIdAsync(int id);
    Task<Student> CreateAsync(Student student);
    Task<Student> UpdateAsync(Student student);
    Task<bool> DeleteAsync(int id);

}