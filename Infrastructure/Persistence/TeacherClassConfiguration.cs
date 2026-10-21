using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testimonial.Domain.Entities;

public class TeacherClassConfiguration : IEntityTypeConfiguration<TeacherClass>
{
    public void Configure(EntityTypeBuilder<TeacherClass> builder)
    {
        builder.HasKey(tc => new { tc.TeacherId, tc.ClassId });

        builder.HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherClasses)
            .HasForeignKey(tc => tc.TeacherId);

        builder.HasOne(tc => tc.Class)
            .WithMany()
            .HasForeignKey(tc => tc.ClassId);
    }
}


