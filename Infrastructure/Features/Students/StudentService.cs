using Testimonial.Application.Features.Students;
using Testimonial.Application.Features.Classes;
using Testimonial.Domain.Entities;
using Testimonial.Application.Features.Students;
using Application.Features.Students;

namespace Testimonial.Infrastructure.Features.Students;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IClassRepository _classRepository;

    public StudentService(IStudentRepository repo, IClassRepository classRepo)
    {
        _studentRepository = repo;
        _classRepository = classRepo;
    }

    public Task<IEnumerable<Student>> GetAllAsync()
        => _studentRepository.GetAllWithClassAsync();

    public Task<Student?> GetByIdAsync(int id)
        => _studentRepository.GetByStudentIdWithClassAsync(id);

    public async Task<StudentDTO?> GetByStudentIdAsync(int id)
    {
        var student = await _studentRepository.GetByStudentIdWithClassAsync(id);

        if (student == null)
            return null;

        return new StudentDTO
        {
            StudentId = student.StudentId,
            FirstName = student.FirstName,
            LastName = student.LastName,
            ClassName = student.Class?.Name
        };
    }

    public async Task<Student> CreateAsync(Student student)
    {
        var classEntity = await _classRepository.GetByIdAsync(student.ClassId);
        if (classEntity == null)
            throw new Exception("Class not found");

        return await _studentRepository.CreateAsync(student);
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        var classEntity = await _classRepository.GetByIdAsync(student.ClassId);
        if (classEntity == null)
            throw new Exception("Class not found");

        return await _studentRepository.UpdateAsync(student);
    }

    public Task<bool> DeleteAsync(int id)
        => _studentRepository.DeleteAsync(id);
}

