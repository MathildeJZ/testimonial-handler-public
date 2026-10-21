namespace Testimonial.Domain.Entities;

public class Testimonial
{
    public int TestimonialId { get; set; }

    public string Title { get; set; } = string.Empty;
    public int SchoolYear { get; set; }
    public string Grade { get; set; } = string.Empty;
    public DateOnly DateCreated { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
    public string OverallStudentDescription { get; set; }
    public string EndMessage {get; set;}  


    public List<SubjectEntry> SubjectEntries { get; set; } = new();
}