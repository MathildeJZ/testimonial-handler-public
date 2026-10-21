namespace Testimonial.Application.Features.Testimonials.Interfaces;

using Testimonial.Domain.Entities;

public interface ITestimonialService
{
    Task<Testimonial> CreateTestimonialAsync(CreateTestimonialRequest request);

    Task<List<TestimonialDto>> GetByStudentId(int studentId);

    Task<IEnumerable<SearchTestimonialResponse>> SearchTestimonialsAsync(SearchTestimonialRequest request);

    Task<IEnumerable<Testimonial?>> GetTestimonialByIdAsync(string id);

    Task<bool> DeleteTestimonialAsync(string id);

    Task<Testimonial?> GetTestimonialsAsync(int studentId);
}
