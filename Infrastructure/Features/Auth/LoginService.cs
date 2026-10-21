using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Testimonial.Infrastructure.Identity;
using Testimonial.Application.Features.Auth;

namespace Testimonial.Infrastructure.Features.Auth;

public sealed class LoginService : ILoginService
{
    //private readonly IIdentityAuthService _identity;
    private readonly IAuthTokenService _tokenService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LoginService> _logger;

    public LoginService(
        //IIdentityAuthService identity,
        IAuthTokenService tokenService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<LoginService> logger
        )
    {
        //_identity = identity;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed: invalid credentials for email {Email}.", request.Email);
            return null;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: false // set true if we want lockout
        );

        if (!signInResult.Succeeded)
        {
            _logger.LogWarning("Login failed: invalid credentials for email {Email}.", request.Email);
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        var (token, expiresAt) = await _tokenService.CreateAccessTokenAsync(
            user.Id,
            user.Email!,
            roles
        );

        return new LoginResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}