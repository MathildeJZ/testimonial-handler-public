namespace Testimonial.Domain.Entities;

public class Teacher
{
    public int TeacherId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public int SubjectEntryId { get; set; }
    public List<SubjectEntry> SubjectEntry { get; set; } = new();
}