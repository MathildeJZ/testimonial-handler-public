//Local Libraries
using Testimonial.Infrastructure.Features.SubjectEntries;
using Testimonial.Application.Features.SubjectEntries;
using Testimonial.Application.Features.Users;
using Testimonial.Infrastructure.Persistence;
using Testimonial.Domain.Entities;
//External libraries
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Testimonial.Infrastructure.Features.SubjectEntries;

public class SubjectEntriesService : ISubjectEntryService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<SubjectEntriesService> _logger;

    public SubjectEntriesService(AppDbContext context, ICurrentUserService currentUser, ILogger<SubjectEntriesService> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<SubjectEntry> CreateSubjetEntryAsync(CreateSubjectEntryRequest request)
    {
        var userId = _currentUser.UserId;
        _logger.LogInformation("🐙🐙CurrentUserId: {UserId}🐙🐙", userId);

        var allTeachers = await _context.Teacher.ToListAsync();
        foreach (var t in allTeachers)
        {
            _logger.LogInformation("👀 Teacher: {TeacherId}, UserId: {UserId}", t.TeacherId, t.UserId);
        }

        var teacher = await _context.Teacher
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (teacher == null)
        {
            throw new Exception($"🔴🔴 CURRENTUSERID: {userId}🔴🔴");
        }
        if (request == null)
        {
            throw new Exception("🔴🔴REQUEST CHECK FAILED🔴🔴");
        }

        var entry = new SubjectEntry
        {
            SubjectId = request.SubjectId,
            StudentId = request.StudentId,
            TeacherId = teacher.TeacherId,

            PersonalStatement = request.PersonalStatement ?? string.Empty,
            SubjectDescription = string.Join("\n",
                request.DescriptionBlocks.Select(b => $"{b.Heading} {b.Body}")
            )
        };

        _context.SubjectEntries.Add(entry);
        await _context.SaveChangesAsync();

        return entry;
    }

    public async Task<List<SubjectEntryDto>> GetByStudentId(int studentId)
    {
        return await _context.SubjectEntries
        .Where(se => se.StudentId == studentId)
        .Select(se => new SubjectEntryDto
        {
            StudentId = se.StudentId,
            PersonalStatement = se.PersonalStatement
        })
        .ToListAsync();
    }
}