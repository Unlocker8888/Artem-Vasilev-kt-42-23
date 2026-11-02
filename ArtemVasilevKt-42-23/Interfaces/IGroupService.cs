using ArtemVasilevKt_42_23.Data;
using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using ArtemVasilevKt_42_23.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtemVasilevKt_42_23.Interfaces;

public interface IGroupService
{
    Task<GroupDto[]> GetGroupsAsync(GroupFilter filter, CancellationToken cancellationToken = default);
    Task<GroupDto?> AddGroupAsync(GroupRequest request, CancellationToken cancellationToken = default);
    Task<GroupDto?> UpdateGroupAsync(int groupId, GroupRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteGroupAsync(int groupId, CancellationToken cancellationToken = default);
}

public class GroupService : IGroupService
{
    private readonly AppDbContext _dbContext;

    public GroupService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GroupDto[]> GetGroupsAsync(GroupFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Groups.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SpecialityCode))
            query = query.Where(g => g.Speciality.Code == filter.SpecialityCode);

        if (filter.Course.HasValue)
            query = query.Where(g => g.Course == filter.Course.Value);

        if (filter.IsDeleted.HasValue)
            query = query.Where(g => g.IsDeleted == filter.IsDeleted.Value);

        return await query
            .Select(g => new GroupDto
            {
                GroupId = g.GroupId,
                Name = g.Name,
                Course = g.Course,
                SpecialityId = g.SpecialityId,
                SpecialityTitle = g.Speciality.Title,
                IsDeleted = g.IsDeleted
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<GroupDto?> AddGroupAsync(GroupRequest request, CancellationToken cancellationToken = default)
    {
        var speciality = await _dbContext.Specialities
            .FirstOrDefaultAsync(s => s.SpecialityId == request.SpecialityId, cancellationToken);
        if (speciality == null)
            return null;

        var group = new Group
        {
            Name = request.Name,
            Course = request.Course,
            SpecialityId = speciality.SpecialityId,
            IsDeleted = false
        };
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToDto(group, speciality);
    }

    public async Task<GroupDto?> UpdateGroupAsync(int groupId, GroupRequest request, CancellationToken cancellationToken = default)
    {
        var group = await _dbContext.Groups
            .FirstOrDefaultAsync(g => g.GroupId == groupId && !g.IsDeleted, cancellationToken);
        if (group == null)
            return null;

        var speciality = await _dbContext.Specialities
            .FirstOrDefaultAsync(s => s.SpecialityId == request.SpecialityId, cancellationToken);
        if (speciality == null)
            return null;

        group.Name = request.Name;
        group.Course = request.Course;
        group.SpecialityId = speciality.SpecialityId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToDto(group, speciality);
    }

    public async Task<bool> DeleteGroupAsync(int groupId, CancellationToken cancellationToken = default)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.GroupId == groupId && !g.IsDeleted, cancellationToken);
        if (group == null)
            return false;

        group.IsDeleted = true;
        foreach (var student in group.Students)
            student.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static GroupDto ToDto(Group g, Speciality s) => new()
    {
        GroupId = g.GroupId,
        Name = g.Name,
        Course = g.Course,
        SpecialityId = s.SpecialityId,
        SpecialityTitle = s.Title,
        IsDeleted = g.IsDeleted
    };
}
