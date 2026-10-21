using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testimonial.Domain.Entities;

namespace Testimonial.Infrastructure.Persistence.Seed;

public static class TestDataSeed
{
    public static async Task SeedAsync(AppDbContext db, ILogger logger)
    {
        var existing = await db.Students.CountAsync();
        logger.LogInformation("Students count before seed: {Count}", existing);

        // one guard is enough
        if (existing > 0)
            return;

        var c = new Class
        {
            Name = "9A",
            Year = 9
        };

        var s = new Student
        {
            FirstName = "Test",
            LastName = "Student",
            School = "Kristofferskolen",
            ClassId = c.ClassId  // your navigation property (Class)
        };

        // you can add just the student; EF will also insert the class
        db.Students.Add(s);

        var saved = await db.SaveChangesAsync();
        logger.LogInformation("Seed complete. SaveChangesAsync wrote {Saved} row(s).", saved);
    }
}
