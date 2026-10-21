namespace Testimonial.Infrastructure.Features.Testimonials;

public class TeacherTitleHekper
{
    public static string GenerateTeacherTitle(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
            return "Lærer";

        subject = subject.Trim();

        if (subject.EndsWith("fag", StringComparison.OrdinalIgnoreCase))
            return subject + "slærer";

        if (subject.EndsWith("t", StringComparison.OrdinalIgnoreCase))
            return subject + "slærer";

        return subject + "lærer";
    }
}