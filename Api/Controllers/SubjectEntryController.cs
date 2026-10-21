//LOCAL LIBRARIES
using Testimonial.Application.Features.SubjectEntries;
using Testimonial.Infrastructure.Persistence;
//EXTERNAL LIBRARIES
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Testimonial.Api.Controllers;

[ApiController]
[Route("api/subjectEntry")]
public class SubjectEntryController : ControllerBase
{
    private readonly ISubjectEntryService _subjectEntryService;
    private readonly AppDbContext _context;

    public SubjectEntryController(ISubjectEntryService subjectEntryService, AppDbContext context)
    {
        _subjectEntryService = subjectEntryService;
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSubjectEntry(CreateSubjectEntryRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _subjectEntryService.CreateSubjetEntryAsync(request);

        return Ok(User.Claims.Select(c => new
        {
            c.Type,
            c.Value
        }));
    }
}