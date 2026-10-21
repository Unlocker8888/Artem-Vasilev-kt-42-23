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
        var students = await _studentService.GetStudentsAsync(filter, cancellationToken);
        return Ok(students);
    }
}