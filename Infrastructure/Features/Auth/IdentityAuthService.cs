using Microsoft.AspNetCore.Identity;
using Testimonial.Infrastructure.Identity;
using Testimonial.Application.Features.Auth;

namespace Testimonial.Infrastructure.Features.Auth;

public class IdentityAuthService : IIdentityAuthService
{
    private readonly UserManager<ApplicationUser> _usermanager;

    public IdentityAuthService(UserManager<ApplicationUser> userManager)
    {
        _usermanager = userManager;
    }

    public async Task<LoginResult> ValidateUserAsync(string email, string password)
    {
        var user = await _usermanager.FindByEmailAsync(email);
        if (user == null) return null;

        var ok = await _usermanager.CheckPasswordAsync(user, password);
        if (!ok) return null;

        var roles = await _usermanager.GetRolesAsync(user);

        Console.WriteLine($"🔥 IDENTITY SERVICE ROLES: {string.Join(",", roles)}");

        return new LoginResult
        {
            UserId = user.Id,
            Email = user.Email ?? email,
            Roles = roles
        };
    }
}