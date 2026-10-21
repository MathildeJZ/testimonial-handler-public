namespace Testimonial.Application.Features.Testimonials;

public class CreateTestimonialRequest
{
    public int StudentId { get; set; }
    public string Title { get; set; }
    public string Grade { get; set; }
    public int SchoolYear { get; set; }

    public string OverallStudentDescription { get; set; } = "";
    public string EndMessage { get; set; } = "";

    public List<SubjectSectionDto> Subjects { get; set; } = new();


}