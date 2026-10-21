using Microsoft.AspNetCore.Mvc;

namespace Testimonial.Api.Controllers
{
    [ApiController]
    [Route("api/selection")]
    public class SelectionController : ControllerBase
    {
        public class SelectionRequest
        {
            public List<int> StudentIds { get; set; }
            public int ClassId { get; set; }
        }

        [HttpPost]
        public IActionResult SaveSelection([FromBody] SelectionRequest request)
        {
            if (request == null)
                return BadRequest("Request is null");

            if (request.StudentIds == null || request.StudentIds.Count == 0)
                return BadRequest("No students selected");

            if (request.ClassId <= 0)
                return BadRequest("Invalid classId");

            return Ok(new
            {
                message = "Selection received",
                students = request.StudentIds,
                classId = request.ClassId
            });
        }
    }
}

    


