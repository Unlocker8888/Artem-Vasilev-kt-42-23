using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using ArtemVasilevKt_42_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtemVasilevKt_42_23.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    // Средний балл по предмету в группе (нужны groupId и disciplineId)
    [HttpPost("GetAverageByDisciplineInGroup")]
    public async Task<ActionResult<AverageDto>> GetAverageByDisciplineInGroupAsync(
        [FromBody] GradeFilter filter, CancellationToken cancellationToken)
    {
        if (!filter.GroupId.HasValue || !filter.DisciplineId.HasValue)
            return BadRequest("Укажите groupId и disciplineId");

        return Ok(await _gradeService.GetAverageByDisciplineInGroupAsync(
            filter.GroupId.Value, filter.DisciplineId.Value, cancellationToken));
    }

    // Оценки конкретного студента (нужен studentId, disciplineId необязателен)
    [HttpPost("GetStudentGrades")]
    public async Task<ActionResult<GradeDto[]>> GetStudentGradesAsync(
        [FromBody] GradeFilter filter, CancellationToken cancellationToken)
    {
        if (!filter.StudentId.HasValue)
            return BadRequest("Укажите studentId");

        return Ok(await _gradeService.GetStudentGradesAsync(
            filter.StudentId.Value, filter.DisciplineId, cancellationToken));
    }

    // Средний балл по году / курсу (нужен course)
    [HttpPost("GetAverageByYear")]
    public async Task<ActionResult<AverageDto>> GetAverageByYearAsync(
        [FromBody] GradeFilter filter, CancellationToken cancellationToken)
    {
        if (!filter.Course.HasValue)
            return BadRequest("Укажите course");

        return Ok(await _gradeService.GetAverageByCourseAsync(filter.Course.Value, cancellationToken));
    }

    [HttpPost("AddGrade")]
    public async Task<ActionResult<GradeDto>> AddGradeAsync(
        [FromBody] GradeRequest request, CancellationToken cancellationToken)
    {
        var result = await _gradeService.AddGradeAsync(request, cancellationToken);
        if (result == null)
            return NotFound("Студент или дисциплина не найдены");
        return Ok(result);
    }

    [HttpPut("UpdateGrade/{gradeId}")]
    public async Task<ActionResult<GradeDto>> UpdateGradeAsync(
        int gradeId, [FromBody] GradeUpdateRequest request, CancellationToken cancellationToken)
    {
        var result = await _gradeService.UpdateGradeAsync(gradeId, request, cancellationToken);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
}
