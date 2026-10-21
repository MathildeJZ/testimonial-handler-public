namespace Testimonial.Domain.Entities;

public class Subject
{
    public int SubjectId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<TeacherSubject> TeacherSubjects { get; set; } = new();

    public int Grade { get; set; }

    public int ClassId { get; set; }

    public List<SubjectEntry> SubjectEntries { get; set; } = new();
}
