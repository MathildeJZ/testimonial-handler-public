// using System.Threading.Tasks;
// using Testimonial.Application.Features.Testimonials.Interfaces;
// using Testimonial.Domain.Entities;

// namespace Testimonial.Application.UseCases;

// public class UpdateTestimonialUseCase : ITestimonialRepository
// {
//     private readonly ITestimonialRepository _repository;

//     public UpdateTestimonialUseCase(ITestimonialRepository repository)
//     {
//         _repository = repository;
//     }

//     public Domain.Entities.Testimonial Execute(int id, Domain.Entities.Testimonial updatedData)
//     {
//         var existing = _repository.GetById(id);
//         if (existing == null)
//             throw new Exception("Testimonial findes ikke");

//         existing.Title = updatedData.Title;
//         existing.Grade = updatedData.Grade;
//         existing.SchoolYear = updatedData.SchoolYear;
//         existing.SubjectEntries = updatedData.SubjectEntries;

//         return _repository.Update(existing);
//     }
// }
