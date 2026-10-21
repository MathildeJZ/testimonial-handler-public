namespace Testimonial.Application.Features.Auth;

public interface ILoginService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}