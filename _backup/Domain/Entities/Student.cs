namespace Testimonial.Domain.Entities;

public class Student
{
    public int StudentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string School { get; set; }
    public DateOnly StartDate { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;

    public List<Testimonial> Testimonials { get; set; } = new();

}