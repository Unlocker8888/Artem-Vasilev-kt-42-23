using System.ComponentModel.DataAnnotations;
using ArtemVasilevKt_42_23.Models;

namespace ArtemVasilevKt_42_23.Dto;

public class GroupDto
{
    public int GroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Course { get; set; }
    public int SpecialityId { get; set; }
    public string SpecialityTitle { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}

public class DisciplineDto
{
    public int DisciplineId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DisciplineDirection Direction { get; set; }
    public bool IsDeleted { get; set; }
}

public class GradeDto
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int DisciplineId { get; set; }
    public string DisciplineName { get; set; } = string.Empty;
    public int Value { get; set; }
}

public class AverageDto
{
    public double? Average { get; set; }
}

public class StudentRequest
{
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    public int GroupId { get; set; }
}

public class GroupRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    [Range(1, 6)] public int Course { get; set; }
    public int SpecialityId { get; set; }
}

public class DisciplineRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    public DisciplineDirection Direction { get; set; }
}

public class GradeRequest
{
    public int StudentId { get; set; }
    public int DisciplineId { get; set; }
    [Range(2, 5)] public int Value { get; set; }
}

public class GradeUpdateRequest
{
    [Range(2, 5)] public int Value { get; set; }
}
