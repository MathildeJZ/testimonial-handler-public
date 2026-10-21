using Microsoft.AspNetCore.Mvc;
using Testimonial.Application.Features.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Testimonial.Infrastructure.Identity;
using System.Diagnostics.Eventing.Reader;
//using Microsoft.AspNetCore.HttpLogging;

namespace Testimonial.Api.Controllers.AuthControllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Testimonial.Application.Features.Auth.ILoginService _loginService;
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthController(Testimonial.Application.Features.Auth.ILoginService loginservice, ILogger<AuthController> logger, UserManager<ApplicationUser> userManager)
    {
        _loginService = loginservice;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        //400 message for malformed request
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid login request" });
        }

        var result = await _loginService.LoginAsync(request);
        _logger.LogInformation("LoginService result: {@result}", result);

        //401 message for wrong credentials 
        if (result == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        //Hent bruger
        var user = await _userManager.FindByEmailAsync(request.Email);
        _logger.LogInformation("User found: {@user}", user);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        try
        {
            var roles = await _userManager.GetRolesAsync(user);

            Console.WriteLine($"🐷🐷AUTH CONTROLLER ROLE USERS ROLE: {string.Join(",", roles)}🐷🐷");

            //200 result for success
            return Ok(new
            {
                token = result.Token,
                userId = user.Id,
                role = roles.FirstOrDefault() ?? "User"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch roles user {Email}", request.Email);
            return StatusCode(500, new { message = "Failed to fetch user roles" });
        }
    }
}