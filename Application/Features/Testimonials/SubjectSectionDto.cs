namespace Testimonial.Application.Features.Testimonials;

public class SubjectSectionDto
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = "";

    public string SubjectDescription { get; set; } = "";

    public List<DescriptionBlockDto> DescriptionBlocks { get; set; } = new();

    public string TeacherStatement { get; set; } = "";

    public string TeacherName { get; set; } = "";
    public int TeacherId { get; set; }
}
