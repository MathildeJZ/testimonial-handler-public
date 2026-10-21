using Testimonial.Application.Features.Testimonials.Interfaces;

namespace Testimonial.Application.UseCases;

public class DeleteTestimonialUseCase
{
    private readonly ITestimonialRepository _repository;

    public DeleteTestimonialUseCase(ITestimonialRepository repository)
    {
        _repository = repository;
    }

    public bool Execute(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID mangler eller er ugyldigt");

        return _repository.Delete(id);
    }
}
