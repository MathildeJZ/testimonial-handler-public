using Testimonial.Domain.Entities;

namespace Testimonial.Application.Features.Classes;

public interface IClassRepository
{
    Task<Class?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Class>> GetAllAsync(CancellationToken ct = default);
}
