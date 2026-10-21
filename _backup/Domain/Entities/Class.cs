
namespace Testimonial.Domain.Entities;

public class Class
{
    public int ClassId;
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }

    public List<Student> Students { get; set; } = new();
}