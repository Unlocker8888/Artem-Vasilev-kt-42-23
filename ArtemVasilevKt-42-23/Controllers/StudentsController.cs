using ArtemVasilevKt_42_23.Dto;
using ArtemVasilevKt_42_23.Filters;
using ArtemVasilevKt_42_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtemVasilevKt_42_23.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost("GetStudents")]
    public async Task<ActionResult<StudentDto[]>> GetStudentsAsync(
        [FromBody] StudentFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await _studentService.GetStudentsAsync(filter, cancellationToken));
    }

    [HttpPost("GetGroupsWithFails")]
    public async Task<ActionResult<string[]>> GetGroupsWithFailsAsync(
        [FromBody] GroupsWithFailsFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await _studentService.GetGroupsWithFailsAsync(filter, cancellationToken));
    }

    [HttpPost("AddStudent")]
    public async Task<ActionResult<StudentDto>> AddStudentAsync(
        [FromBody] StudentRequest request, CancellationToken cancellationToken)
    {
        var result = await _studentService.AddStudentAsync(request, cancellationToken);
        if (result == null)
            return NotFound("Группа не найдена или удалена");
        return Ok(result);
    }

    [HttpPut("UpdateStudent/{studentId}")]
    public async Task<ActionResult<StudentDto>> UpdateStudentAsync(
        int studentId, [FromBody] StudentRequest request, CancellationToken cancellationToken)
    {
        var result = await _studentService.UpdateStudentAsync(studentId, request, cancellationToken);
        if (result == null)
            return NotFound("Студент или группа не найдены");
        return Ok(result);
    }

    [HttpDelete("DeleteStudent/{studentId}")]
    public async Task<IActionResult> DeleteStudentAsync(int studentId, CancellationToken cancellationToken)
    {
        var deleted = await _studentService.DeleteStudentAsync(studentId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
