using Testimonial.Domain.Authentication;

namespace Testimonial.Application.Features.Auth;

public interface IAuthTokenService
{
    Task<(string token, DateTime expiresAt)> CreateAccessTokenAsync(
        string userId,
        string email,
        IList<string> roles);
    string CreateToken(string userId, string email, IList<string> roles);
}