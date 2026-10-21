namespace Application.Features.Students;

public class StudentDTO
{
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ClassName { get; set; } //bliver til student.Class.Name ~ til display
    public int ClassId { get; set; } // til navigation/editing
    public bool HasUploaded{get; set;}
}
