using Testimonial.Domain.Entities;

namespace Testimonial.Application.Features.Interface;

    public interface ISubjectService
{
    Task<IEnumerable<Subject>> GetSubjectsForTestimonial(string userId, string testimonial);
}

