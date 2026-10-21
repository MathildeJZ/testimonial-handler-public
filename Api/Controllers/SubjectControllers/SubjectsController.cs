using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testimonial.Infrastructure.Persistence;

namespace Testimonial.Api.Controllers.SubjectControllers;

[ApiController]
[Route("api/subjects")]

public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet("{grade}")]
    public async Task<IActionResult> GetSubjectsByGrade(int grade)
    {
        var subjects = await _context.Subjects
            .Where(s => s.Grade == grade)
            .Select(s => new
            {
                subjectId = s.SubjectId,
                name = s.Name,

            })
            .ToListAsync();
        return Ok(subjects);
    }
}