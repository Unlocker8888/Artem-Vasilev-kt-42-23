using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using ArtemVasilevKt_42_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtemVasilevKt_42_23.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DisciplinesController : ControllerBase
{
    private readonly IDisciplineService _disciplineService;

    public DisciplinesController(IDisciplineService disciplineService)
    {
        _disciplineService = disciplineService;
    }

    [HttpPost("GetDisciplines")]
    public async Task<ActionResult<DisciplineDto[]>> GetDisciplinesAsync(
        [FromBody] DisciplineFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await _disciplineService.GetDisciplinesAsync(filter, cancellationToken));
    }

    [HttpPost("AddDiscipline")]
    public async Task<ActionResult<DisciplineDto>> AddDisciplineAsync(
        [FromBody] DisciplineRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _disciplineService.AddDisciplineAsync(request, cancellationToken));
    }

    [HttpPut("UpdateDiscipline/{disciplineId}")]
    public async Task<ActionResult<DisciplineDto>> UpdateDisciplineAsync(
        int disciplineId, [FromBody] DisciplineRequest request, CancellationToken cancellationToken)
    {
        var result = await _disciplineService.UpdateDisciplineAsync(disciplineId, request, cancellationToken);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("DeleteDiscipline/{disciplineId}")]
    public async Task<IActionResult> DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken)
    {
        var deleted = await _disciplineService.DeleteDisciplineAsync(disciplineId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
