namespace Testimonial.Application.Features.Users;

public interface ICurrentUserService
{
    string UserId { get; }
    string Role { get; }
}