using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Testimonial.Application.Features.Users;

namespace Testimonial.Infrastructure.Features.Users;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;



    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }



    public string UserId =>
        _httpContextAccessor.HttpContext?
        .User?
        .FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? _httpContextAccessor.HttpContext?.User?
        .FindFirst("sub")?
        .Value!;

    public string Role =>
        _httpContextAccessor.HttpContext?
        .User?
        .Claims
        .FirstOrDefault(c => c.Type.Contains("role"))?
        .Value!;
}