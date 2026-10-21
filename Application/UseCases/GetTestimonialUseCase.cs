using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Domain.Entities;

namespace Testimonial.Application.UseCases;

public class GetTestimonialUseCase
{
    private readonly ITestimonialRepository _repository;

    public GetTestimonialUseCase(ITestimonialRepository repository)
    {
        _repository = repository;
    }

    public Testimonial.Domain.Entities.Testimonial? Execute(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id mangler eller er ugyldigt");

        return _repository.GetById(id);
    }
}


