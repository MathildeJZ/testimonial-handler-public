using Microsoft.AspNetCore.Mvc;
using Testimonial.Application.Features.Interface;


namespace Testimonial.Api.Controllers.SubjectControllers;

[ApiController]
[Route("api/subject")]

public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;

    }

    [HttpDelete("{userId}/{testimonial}")]
    public async Task<IActionResult> GetSubjects(string userId, string testimonial)
    {
        var subject = await _subjectService.GetSubjectsForTestimonial(userId, testimonial);

        if (_subjectService == null || !subject.Any())
            return NotFound("Intet fag fundet for denne klasse");

        return Ok(subject);

    }


}