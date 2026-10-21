namespace Testimonial.Domain.Entities;

public class Subject
{
    public int SubjectId { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<SubjectEntry> SubjectEntries { get; set; } = new();
}