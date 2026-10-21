using System.Reflection.Metadata.Ecma335;

namespace Testimonial.Application.Features.Testimonials.Interfaces;

public interface ITestimonialPdfGenerator
{
    byte[] Generate(TestimonialDto dto);
    // {
    //     var doc = new TestimonialDocument(dto);
    //     ReturnTypeEncoder doc.GeneratePdf();
    // }
}
