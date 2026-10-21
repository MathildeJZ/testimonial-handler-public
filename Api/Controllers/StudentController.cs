using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testimonial.Infrastructure.Persistence;
using Testimonial.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Testimonial.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Testimonial.Application.Features.Students;
[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly AppDbContext _context;

    public StudentController(IStudentService studentService, AppDbContext context)
    {
        _studentService = studentService;
        _context = context;
    }

    // GET: api/students/by-class/3
    [HttpGet("by-class/{classId}")]
    public async Task<IActionResult> GetByClass(int classId)
    {
        var students = await _context.Students
            .Where(s => s.ClassId == classId)
            .Select(s => new
            {
                studentId = s.StudentId,
                firstName = s.FirstName ?? "",
                lastName = s.LastName ?? "",
                classId = s.ClassId,
                school = s.School ?? "",
                hasUploaded = _context.Testimonials.Any(t => t.StudentId == s.StudentId)
            })
            .ToListAsync();

        return Ok(students);
    }

    // GET: api/students
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var testimonials = await _context.Testimonials.ToListAsync();

        var students = await _context.Students
            .Select(s => new
            {
                studentId = s.StudentId,
                firstName = s.FirstName,
                lastName = s.LastName,
                classId = s.ClassId,
                hasUploaded = testimonials.Any(t => t.StudentId == s.StudentId)
            })
            .ToListAsync();

        return Ok(students);
    }


    // GET: api/students/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetByStudentIdAsync(id);

        if (student == null)
            return NotFound("Student not found");

        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Ok(student);
    }

    // PUT: api/students/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Student updated)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound("Student not found");

        student.FirstName = updated.FirstName;
        student.LastName = updated.LastName;
        student.School = updated.School;
        student.ClassId = updated.ClassId;

        await _context.SaveChangesAsync();

        return Ok(student);
    }

    // DELETE: api/students/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students
            .Include(s => s.Testimonials)
            .ThenInclude(t => t.SubjectEntries)
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (student == null)
            return NotFound("Elev ikke fundet");

        foreach (var testimonial in student.Testimonials)
        {
            _context.SubjectEntries.RemoveRange(testimonial.SubjectEntries);
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return Ok("Elev slettet");
    }
}
