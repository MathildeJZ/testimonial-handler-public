using System.Security.Claims;
using Testimonial.Application.Features.Auth;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Testimonial.Infrastructure.Features.Auth;


public sealed class AuthTokenService : IAuthTokenService
{
    private readonly IConfiguration _config;

    public AuthTokenService(IConfiguration config)
    {
        _config = config;
    }

    //public sealed record LoginResponse(string AccessToken, DateTime ExpiresAt);
    public string CreateToken(string userId, string email, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email)
        }; ;

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return string.Empty;
    }

    public Task<(string token, DateTime expiresAt)> CreateAccessTokenAsync(
        string userId,
        string email,
        IList<string> roles)
    {
        var jwt = _config.GetSection("Jwt");

        var issuer = jwt["issuer"]!;
        var audience = jwt["audience"]!;
        var key = jwt["key"]!;
        var expiresMinutes = int.Parse(jwt["ExpiresMinutes"] ?? "60");

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            //who dis claim?
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, email)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)); //SigningUserkEy something instead??
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult((tokenString, expiresAt));
    }
}