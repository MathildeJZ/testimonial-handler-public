using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Testimonial.Domain.Entities;
using Testimonial.Infrastructure.Identity;

namespace Testimonial.Infrastructure.Persistence;

public sealed class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Domain.Entities.Testimonial> Testimonials { get; set; }
    public DbSet<SubjectEntry> SubjectEntries { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<TeacherSubject> TeacherSubject { get; set; }
    public DbSet<TeacherClass> TeacherClasses { get; set; }


    // ⭐ NYT: UserClass relation

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return; // Don't override DI/factory settings
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // -----------------------------
        // Testimonial → SubjectEntries
        // -----------------------------
        modelBuilder.Entity<Domain.Entities.Testimonial>()
            .HasMany(t => t.SubjectEntries)
            .WithOne(se => se.Testimonial)
            .HasForeignKey(se => se.TestimonialId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // Teacher → ApplicationUser
        // -----------------------------
        modelBuilder.Entity<Teacher>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Teacher>(t => t.UserId)
            .HasPrincipalKey<ApplicationUser>(u => u.Id);

        // -----------------------------
        // SubjectEntry → Testimonial
        // -----------------------------
        modelBuilder.Entity<SubjectEntry>()
            .HasOne(se => se.Testimonial)
            .WithMany(t => t.SubjectEntries)
            .HasForeignKey(se => se.TestimonialId)
            .HasConstraintName("FK_SubjectEntries_Testimonials_TestimonialId");

        // -----------------------------
        // Student → Testimonials
        // -----------------------------
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Testimonials)
            .WithOne(t => t.Student)
            .HasForeignKey(t => t.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // TeacherSubject (many-to-many)
        // -----------------------------
        modelBuilder.Entity<TeacherSubject>()
            .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

        modelBuilder.Entity<Teacher>().ToTable("Teacher");

        // -----------------------------
        //teacher many to many relationship with class
        //---------------
        modelBuilder.Entity<TeacherClass>()
        .HasKey(tc => new { tc.TeacherId, tc.ClassId });

        modelBuilder.Entity<TeacherClass>()
            .HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherClasses)
            .HasForeignKey(tc => tc.TeacherId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        modelBuilder.Entity<TeacherClass>()
            .HasOne(tc => tc.Class)
            .WithMany()
            .HasForeignKey(tc => tc.ClassId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);




        // Seed data
        // -----------------------------
        modelBuilder.Entity<Subject>().HasData(
            new Subject { SubjectId = 1, Name = "Elevbeskrivelse", Grade = 2 },

            new Subject { SubjectId = 2, Name = "Idræt", Grade = 4 },
            new Subject { SubjectId = 3, Name = "Musik", Grade = 4 },
            new Subject { SubjectId = 4, Name = "Orkester", Grade = 4 },
            new Subject { SubjectId = 5, Name = "Tysk", Grade = 4 },
            new Subject { SubjectId = 6, Name = "Nordisk mytologi", Grade = 4 },
            new Subject { SubjectId = 7, Name = "Dyrekundskab", Grade = 4 },
            new Subject { SubjectId = 8, Name = "Geografi", Grade = 4 },

            new Subject { SubjectId = 9, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 10, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 11, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 12, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 13, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 14, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 15, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 16, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 17, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 18, Name = "Geografi", Grade = 6 },
            new Subject { SubjectId = 19, Name = "Geografi", Grade = 6 },

            new Subject { SubjectId = 20, Name = "Geografi", Grade = 9 },
            new Subject { SubjectId = 21, Name = "Biologi", Grade = 9 },
            new Subject { SubjectId = 22, Name = "Dansk", Grade = 9 },
            new Subject { SubjectId = 23, Name = "Engelsk", Grade = 9 },
            new Subject { SubjectId = 24, Name = "Eurytmi", Grade = 9 },
            new Subject { SubjectId = 25, Name = "Fysik", Grade = 9 },
            new Subject { SubjectId = 26, Name = "Havebrug", Grade = 9 },
            new Subject { SubjectId = 27, Name = "Historie og samfundsfag", Grade = 9 },
            new Subject { SubjectId = 28, Name = "Håndværk", Grade = 9 },
            new Subject { SubjectId = 29, Name = "Kemi", Grade = 9 },
            new Subject { SubjectId = 30, Name = "Kunstbetragtning", Grade = 9 },
            new Subject { SubjectId = 31, Name = "Matematik", Grade = 9 },
            new Subject { SubjectId = 32, Name = "Orkester og kor", Grade = 9 },
            new Subject { SubjectId = 33, Name = "Skuespil", Grade = 9 }
        );
    }
}
