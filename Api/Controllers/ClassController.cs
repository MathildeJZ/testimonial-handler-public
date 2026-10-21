using Microsoft.AspNetCore.Mvc;
using Testimonial.Application.Features.Classes;
using Testimonial.Domain.Entities;

namespace Testimonial.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassController : ControllerBase
{
    private readonly IClassRepository _classRepository;

    public ClassController(IClassRepository classRepository)
    {
        _classRepository = classRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var classes = await _classRepository.GetAllAsync();
        return Ok(classes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var classEntity = await _classRepository.GetByIdAsync(id);
        if (classEntity == null)
            return NotFound("Class not found");

        return Ok(classEntity);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Class classEntity)
    {
        return BadRequest("Not implemented");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Class classEntity)
    {
        if (id != classEntity.ClassId)
            return BadRequest("ID mismatch");

        return BadRequest("UpdateAsync mangler i IClassRepository");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return BadRequest("DeleteAsync mangler i IClassRepository");
    }
}