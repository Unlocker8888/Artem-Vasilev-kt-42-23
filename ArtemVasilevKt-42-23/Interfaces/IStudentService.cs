using ArtemVasilevKt_42_23.Data;
using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using Microsoft.EntityFrameworkCore;

namespace ArtemVasilevKt_42_23.Interfaces;

public interface IStudentService
{
    Task<StudentDto[]> GetStudentsAsync(StudentFilter filter, CancellationToken cancellationToken = default);
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
}