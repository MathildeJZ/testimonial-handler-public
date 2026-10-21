using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testimonial.Infrastructure.Persistence;
using Testimonial.Domain.Entities;

namespace Testimonial.Api.Controllers;

[ApiController]
[Route("api/teachers")]
public class TeacherController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }

    // Hent alle undervisere
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var teachers = await _context.Teacher.ToListAsync();
            return Ok(teachers);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Failed to fetch teachers" });
        }
    }

    // Opret underviser
    [HttpPost]
    public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
    {
        if (teacher == null)
            return BadRequest("Teacher data is missing");

        teacher.UserId = Guid.NewGuid().ToString();

        _context.Teacher.Add(teacher);
        await _context.SaveChangesAsync();

        return Ok(teacher);
    }
    // Opdater underviser
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher updated)
    {
        var teacher = await _context.Teacher.FindAsync(id);
        if (teacher == null)
            return NotFound();

        teacher.FirstName = updated.FirstName;
        teacher.LastName = updated.LastName;
        teacher.Email = updated.Email;
        teacher.IsClassTeacher = updated.IsClassTeacher;
        teacher.IsSubjectTeacher = updated.IsSubjectTeacher;

        await _context.SaveChangesAsync();
        return Ok(teacher);
    }

    // Slet underviser
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var teacher = await _context.Teacher.FindAsync(id);
        if (teacher == null)
            return NotFound();

        _context.Teacher.Remove(teacher);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
