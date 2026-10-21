namespace Testimonial.Application.Features.Auth;

public interface IIdentityAuthService
{
    Task<LoginResult?> ValidateUserAsync(string email, string password);
}
