using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Testimonial.Infrastructure.Persistence; // ⭐ vigtigt: dette gør UserClass synlig

namespace Testimonial.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? ProfileImageUrl { get; set; }

    [NotMapped]
    public string Name => UserName ?? "";

}
