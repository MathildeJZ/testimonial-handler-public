using Testimonial.Application.Features.Testimonials;

namespace Testimonial.Application.Features;

public class TestimonialDto
{
    public string StudentFullName { get; set; } = "";
    public int Year { get; set; }
    public string ClassName { get; set; } = ""; //?? necessary?
    public string SchoolName { get; set; } = "";
    public string DocumentMonthYear { get; set; } = "";

    public string IntroParagraph { get; set; } = "";
    public string OverallStudentDescription { get; set; } = "";
    public string EndMessage { get; set; } = "";

    public List<SubjectSectionDto> Subjects { get; set; } = new();
}