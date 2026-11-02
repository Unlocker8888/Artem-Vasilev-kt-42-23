using ArtemVasilevKt_42_23.Data;
using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using ArtemVasilevKt_42_23.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtemVasilevKt_42_23.Interfaces;

public interface IDisciplineService
{
    Task<DisciplineDto[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken = default);
    Task<DisciplineDto> AddDisciplineAsync(DisciplineRequest request, CancellationToken cancellationToken = default);
    Task<DisciplineDto?> UpdateDisciplineAsync(int disciplineId, DisciplineRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken = default);
}

public class DisciplineService : IDisciplineService
{
    private readonly AppDbContext _dbContext;

    public DisciplineService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DisciplineDto[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Disciplines.AsNoTracking().AsQueryable();

        if (filter.Direction.HasValue)
            query = query.Where(d => d.Direction == filter.Direction.Value);

        if (filter.IsDeleted.HasValue)
            query = query.Where(d => d.IsDeleted == filter.IsDeleted.Value);

        return await query
            .Select(d => new DisciplineDto
            {
                DisciplineId = d.DisciplineId,
                Name = d.Name,
                Direction = d.Direction,
                IsDeleted = d.IsDeleted
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<DisciplineDto> AddDisciplineAsync(DisciplineRequest request, CancellationToken cancellationToken = default)
    {
        var discipline = new Discipline
        {
            Name = request.Name,
            Direction = request.Direction,
            IsDeleted = false
        };
        _dbContext.Disciplines.Add(discipline);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(discipline);
    }

    public async Task<DisciplineDto?> UpdateDisciplineAsync(int disciplineId, DisciplineRequest request, CancellationToken cancellationToken = default)
    {
        var discipline = await _dbContext.Disciplines
            .FirstOrDefaultAsync(d => d.DisciplineId == disciplineId && !d.IsDeleted, cancellationToken);
        if (discipline == null)
            return null;

        discipline.Name = request.Name;
        discipline.Direction = request.Direction;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(discipline);
    }

    public async Task<bool> DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken = default)
    {
        var discipline = await _dbContext.Disciplines
            .FirstOrDefaultAsync(d => d.DisciplineId == disciplineId && !d.IsDeleted, cancellationToken);
        if (discipline == null)
            return false;

        discipline.IsDeleted = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static DisciplineDto ToDto(Discipline d) => new()
    {
        DisciplineId = d.DisciplineId,
        Name = d.Name,
        Direction = d.Direction,
        IsDeleted = d.IsDeleted
    };
}
