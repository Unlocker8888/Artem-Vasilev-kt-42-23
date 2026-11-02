using ArtemVasilevKt_42_23.Models;

namespace ArtemVasilevKt_42_23.Filters;

public class GroupFilter
{
    public string? SpecialityCode { get; set; }
    public int? Course { get; set; }
    public bool? IsDeleted { get; set; }
}

public class DisciplineFilter
{
    public DisciplineDirection? Direction { get; set; }
    public bool? IsDeleted { get; set; }
}

public class GradeFilter
{
    public int? GroupId { get; set; }
    public int? DisciplineId { get; set; }
    public int? StudentId { get; set; }
    public int? Course { get; set; }
}
