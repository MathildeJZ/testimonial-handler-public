namespace Testimonial.Application.Features.Testimonials.Interfaces;

public interface ITestimonialRepository
{
    Task AddAsync(Domain.Entities.Testimonial testimonial);

    Task<Domain.Entities.Testimonial?> GetByStudentId(int studentId, CancellationToken ct = default);

    Domain.Entities.Testimonial? GetById(int id);

    Task SaveChangesAsync();


    bool Delete(int id);

    Task<SearchTestimonialResponse> SearchAsync(SearchTestimonialRequest request);
}
