namespace Testimonial.Domain.Entities;

public class Class
{
    public int ClassId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }

    public int ClassTeacherId { get; set; }
    public Teacher ClassTeacher { get; set; } = null!;
    public List<Student> Students { get; set; } = new();
}

