using ArtemVasilevKt_42_23.Data;
using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using ArtemVasilevKt_42_23.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtemVasilevKt_42_23.Interfaces;

public interface IStudentService
{
    Task<StudentDto[]> GetStudentsAsync(StudentFilter filter, CancellationToken cancellationToken = default);
    Task<string[]> GetGroupsWithFailsAsync(GroupsWithFailsFilter filter, CancellationToken cancellationToken = default);
    Task<StudentDto?> AddStudentAsync(StudentRequest request, CancellationToken cancellationToken = default);
    Task<StudentDto?> UpdateStudentAsync(int studentId, StudentRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteStudentAsync(int studentId, CancellationToken cancellationToken = default);
}

public class StudentService : IStudentService
{
    private readonly AppDbContext _dbContext;

    public StudentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StudentDto[]> GetStudentsAsync(StudentFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Students.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.GroupName))
            query = query.Where(s => s.Group.Name == filter.GroupName);

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            query = query.Where(s => s.FirstName == filter.FirstName);

        if (!string.IsNullOrWhiteSpace(filter.LastName))
            query = query.Where(s => s.LastName == filter.LastName);

        if (filter.IsDeleted.HasValue)
            query = query.Where(s => s.IsDeleted == filter.IsDeleted.Value);

        return await query
            .Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                GroupName = s.Group.Name,
                IsDeleted = s.IsDeleted
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<string[]> GetGroupsWithFailsAsync(GroupsWithFailsFilter filter, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Groups
            .AsNoTracking()
            .Where(g => !g.IsDeleted && g.Students.Any(s => !s.IsDeleted && s.Grades.Any(gr =>
                gr.Value == 2 && gr.Discipline.Name == filter.DisciplineName)))
            .Select(g => g.Name)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<StudentDto?> AddStudentAsync(StudentRequest request, CancellationToken cancellationToken = default)
    {
        var group = await _dbContext.Groups
            .FirstOrDefaultAsync(g => g.GroupId == request.GroupId && !g.IsDeleted, cancellationToken);
        if (group == null)
            return null;

        var student = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            GroupId = group.GroupId,
            IsDeleted = false
        };
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToDto(student, group.Name);
    }

    public async Task<StudentDto?> UpdateStudentAsync(int studentId, StudentRequest request, CancellationToken cancellationToken = default)
    {
        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsDeleted, cancellationToken);
        if (student == null)
            return null;

        var group = await _dbContext.Groups
            .FirstOrDefaultAsync(g => g.GroupId == request.GroupId && !g.IsDeleted, cancellationToken);
        if (group == null)
            return null;

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.GroupId = group.GroupId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToDto(student, group.Name);
    }

    // Мягкое удаление: запись остаётся в БД, IsDeleted = true
    public async Task<bool> DeleteStudentAsync(int studentId, CancellationToken cancellationToken = default)
    {
        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsDeleted, cancellationToken);
        if (student == null)
            return false;

        student.IsDeleted = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static StudentDto ToDto(Student s, string groupName) => new()
    {
        StudentId = s.StudentId,
        FirstName = s.FirstName,
        LastName = s.LastName,
        GroupName = groupName,
        IsDeleted = s.IsDeleted
    };
}
