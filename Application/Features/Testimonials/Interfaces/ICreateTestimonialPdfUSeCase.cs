namespace Testimonial.Application.Features.Testimonials.Interfaces;

public interface ICreateTestimonialPdfUseCase
{
    Task<byte[]> ExecuteAsync(int studentId, TestimonialDto dto);
}