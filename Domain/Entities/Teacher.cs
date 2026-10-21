using System.Text.Json.Serialization;
using Testimonial.Domain.Entities;

namespace Testimonial.Domain.Entities;

public class Teacher
{
    public int TeacherId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsSubjectTeacher { get; set; }
    public bool IsClassTeacher { get; set; }

    public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

    public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();

    [JsonIgnore]
    public List<SubjectEntry> SubjectEntries { get; set; } = new();

}



