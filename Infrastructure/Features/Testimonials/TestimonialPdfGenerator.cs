using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Application.Features;
using QuestPDF.Fluent;

namespace Testimonial.Infrastructure.Features.Testimonials;

public class TestimonialPdfGenerator : ITestimonialPdfGenerator
{
    public byte[] Generate(TestimonialDto dto)
    {
        var document = new TestimonialDocument(dto);
        return document.GeneratePdf();
    }
}
