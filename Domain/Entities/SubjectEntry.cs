using System.Text.Json.Serialization;

namespace Testimonial.Domain.Entities;

public class SubjectEntry
{
    public int SubjectEntryId { get; set; }

    public string SubjectDescription { get; set; } = string.Empty;
    public string PersonalStatement { get; set; } = string.Empty;
    public string LessonInfo { get; set; } = string.Empty;

    public int OrderIndex { get; set; }

    public int TeacherId { get; set; }
    public int StudentId { get; set; }

    [JsonIgnore]
    public Teacher Teacher { get; set; } = null!;

    public int TestimonialId { get; set; }

    [JsonIgnore]
    public Testimonial Testimonial { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
}
