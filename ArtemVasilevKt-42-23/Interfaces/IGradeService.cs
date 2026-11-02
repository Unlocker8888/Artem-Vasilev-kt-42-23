using ArtemVasilevKt_42_23.Data;
using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtemVasilevKt_42_23.Interfaces;

public interface IGradeService
{
    Task<AverageDto> GetAverageByDisciplineInGroupAsync(int groupId, int disciplineId, CancellationToken cancellationToken = default);
    Task<GradeDto[]> GetStudentGradesAsync(int studentId, int? disciplineId, CancellationToken cancellationToken = default);
    Task<AverageDto> GetAverageByCourseAsync(int course, CancellationToken cancellationToken = default);
    Task<GradeDto?> AddGradeAsync(GradeRequest request, CancellationToken cancellationToken = default);
    Task<GradeDto?> UpdateGradeAsync(int gradeId, GradeUpdateRequest request, CancellationToken cancellationToken = default);
}

public class GradeService : IGradeService
{
    private readonly AppDbContext _dbContext;

    public GradeService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Средний балл по предмету в группе
    public async Task<AverageDto> GetAverageByDisciplineInGroupAsync(int groupId, int disciplineId, CancellationToken cancellationToken = default)
    {
        var average = await _dbContext.Grades
            .AsNoTracking()
            .Where(g => g.DisciplineId == disciplineId
                        && g.Student.GroupId == groupId
                        && !g.Student.IsDeleted)
            .Select(g => (double?)g.Value)
            .AverageAsync(cancellationToken);

        return new AverageDto { Average = average };
    }

    // Оценки конкретного студента (по всем предметам или по одному)
    public async Task<GradeDto[]> GetStudentGradesAsync(int studentId, int? disciplineId, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Grades.AsNoTracking().Where(g => g.StudentId == studentId);

        if (disciplineId.HasValue)
            query = query.Where(g => g.DisciplineId == disciplineId.Value);

        return await Project(query).ToArrayAsync(cancellationToken);
    }

    // Средний балл по году (курсу)
    public async Task<AverageDto> GetAverageByCourseAsync(int course, CancellationToken cancellationToken = default)
    {
        var average = await _dbContext.Grades
            .AsNoTracking()
            .Where(g => g.Student.Group.Course == course
                        && !g.Student.IsDeleted
                        && !g.Student.Group.IsDeleted)
            .Select(g => (double?)g.Value)
            .AverageAsync(cancellationToken);

        return new AverageDto { Average = average };
    }

    public async Task<GradeDto?> AddGradeAsync(GradeRequest request, CancellationToken cancellationToken = default)
    {
        var studentExists = await _dbContext.Students
            .AnyAsync(s => s.StudentId == request.StudentId && !s.IsDeleted, cancellationToken);
        var disciplineExists = await _dbContext.Disciplines
            .AnyAsync(d => d.DisciplineId == request.DisciplineId && !d.IsDeleted, cancellationToken);
        if (!studentExists || !disciplineExists)
            return null;

        var grade = new Grade
        {
            StudentId = request.StudentId,
            DisciplineId = request.DisciplineId,
            Value = request.Value
        };
        _dbContext.Grades.Add(grade);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await Project(_dbContext.Grades.AsNoTracking().Where(g => g.GradeId == grade.GradeId))
            .FirstAsync(cancellationToken);
    }

    public async Task<GradeDto?> UpdateGradeAsync(int gradeId, GradeUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var grade = await _dbContext.Grades
            .FirstOrDefaultAsync(g => g.GradeId == gradeId, cancellationToken);
        if (grade == null)
            return null;

        grade.Value = request.Value;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await Project(_dbContext.Grades.AsNoTracking().Where(g => g.GradeId == gradeId))
            .FirstAsync(cancellationToken);
    }

    private static IQueryable<GradeDto> Project(IQueryable<Grade> query) =>
        query.Select(g => new GradeDto
        {
            GradeId = g.GradeId,
            StudentId = g.StudentId,
            StudentName = g.Student.LastName + " " + g.Student.FirstName,
            DisciplineId = g.DisciplineId,
            DisciplineName = g.Discipline.Name,
            Value = g.Value
        });
}
