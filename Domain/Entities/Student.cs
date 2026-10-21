using System.Text.Json.Serialization;

namespace Testimonial.Domain.Entities;

public class Student
{
    public int StudentId { get; set; }
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    public string? School { get; set; } 

    //Relations
    public int ClassId { get; set; }
    //Navigation -må gerne være null ved EF binding men ikke i domæne
    /// EF Core kræver, at navigation properties er nullable, hvis de ikke skal være required i JSON. Class kan stadig ikke er null i din service‑logik

    [JsonIgnore]
    public Class? Class { get; set; }

    public List<Testimonial> Testimonials { get; set; } = new();
}