namespace Testimonial.Application.Features.Auth;

public class LoginResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; init; }
}