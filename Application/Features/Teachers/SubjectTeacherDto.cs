using Testimonial.Application.Features.Subjects;

namespace Testimonial.Application.Features.Teachers;

public class SubjectTeacherDto
{
    public int TeacherId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SubjectDto> Subjects { get; set; } = new();
}