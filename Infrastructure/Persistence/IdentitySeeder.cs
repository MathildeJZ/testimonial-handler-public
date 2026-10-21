using Microsoft.AspNetCore.Identity;
using Testimonial.Infrastructure.Identity;
using Testimonial.Domain.Authentication;

namespace Testimonial.Infrastructure.Persistence;

public class IdentitySeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var roles = new[]
        {
            RoleNames.Admin,
            RoleNames.ClassTeacher,
            RoleNames.SubjectTeacher
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@test.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);


        var classTeacherEmail = "classteacher@test.com";
        var classteacher = await userManager.FindByEmailAsync(classTeacherEmail);

        var subjectTeacherEmail = "subjectteacher@test.com";
        var subjectteacher = await userManager.FindByEmailAsync(subjectTeacherEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "AdminSeeder",
                LastName = "AdminSeeder"
            };
            var result = await userManager.CreateAsync(admin, "Password123!");
            if (!result.Succeeded)
                throw new Exception("Failed to create admin: " +
                    string.Join(",", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(admin, RoleNames.Admin);
        }

        if (classteacher == null)
        {
            classteacher = new ApplicationUser
            {
                UserName = classTeacherEmail,
                Email = classTeacherEmail,
                FirstName = "ClassTeacherSeeder",
                LastName = "ClassTeacherSeeder"
            };
            var result = await userManager.CreateAsync(classteacher, "Password123!");
            if (!result.Succeeded)
                throw new Exception("Failed to create classTeacher: " +
                    string.Join(",", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(classteacher, RoleNames.ClassTeacher);
        }

        if (subjectteacher == null)
        {
            subjectteacher = new ApplicationUser
            {
                UserName = subjectTeacherEmail,
                Email = subjectTeacherEmail,
                FirstName = "SubjectTeacherSeeder",
                LastName = "SubjectTeacherSeeder"
            };
            var result = await userManager.CreateAsync(subjectteacher, "Password123!");
            if (!result.Succeeded)
                throw new Exception("Failed to create subjectTeacher: " +
                    string.Join(",", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(subjectteacher, RoleNames.SubjectTeacher);
        }
    }
}



