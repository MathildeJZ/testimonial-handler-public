using Testimonial.Application.Features.Testimonials;

namespace Testimonial.Application.Features.SubjectEntries;

public class CreateSubjectEntryRequest
{

    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public string? PersonalStatement { get; set; }

    public List<DescriptionBlockDto> DescriptionBlocks { get; set; }
}
