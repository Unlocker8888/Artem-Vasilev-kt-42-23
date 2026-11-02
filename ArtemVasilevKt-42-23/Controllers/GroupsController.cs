using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using ArtemVasilevKt_42_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtemVasilevKt_42_23.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost("GetGroups")]
    public async Task<ActionResult<GroupDto[]>> GetGroupsAsync(
        [FromBody] GroupFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.GetGroupsAsync(filter, cancellationToken));
    }

    [HttpPost("AddGroup")]
    public async Task<ActionResult<GroupDto>> AddGroupAsync(
        [FromBody] GroupRequest request, CancellationToken cancellationToken)
    {
        var result = await _groupService.AddGroupAsync(request, cancellationToken);
        if (result == null)
            return NotFound("Специальность не найдена");
        return Ok(result);
    }

    [HttpPut("UpdateGroup/{groupId}")]
    public async Task<ActionResult<GroupDto>> UpdateGroupAsync(
        int groupId, [FromBody] GroupRequest request, CancellationToken cancellationToken)
    {
        var result = await _groupService.UpdateGroupAsync(groupId, request, cancellationToken);
        if (result == null)
            return NotFound("Группа или специальность не найдены");
        return Ok(result);
    }

    [HttpDelete("DeleteGroup/{groupId}")]
    public async Task<IActionResult> DeleteGroupAsync(int groupId, CancellationToken cancellationToken)
    {
        var deleted = await _groupService.DeleteGroupAsync(groupId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
