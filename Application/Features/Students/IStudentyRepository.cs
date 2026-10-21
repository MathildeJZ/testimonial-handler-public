using Testimonial.Domain.Entities;

namespace Testimonial.Application.Features.Students;

public interface IStudentRepository
{
    Task<Student?> GetByStudentIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Student>> GetAllAsync(CancellationToken ct = default);
    Task<Student?> GetByStudentIdWithClassAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Student>> GetAllWithClassAsync(CancellationToken ct = default);

    Task<Student> CreateAsync(Student student, CancellationToken ct = default);
    Task<Student> UpdateAsync(Student student, CancellationToken ct=default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}