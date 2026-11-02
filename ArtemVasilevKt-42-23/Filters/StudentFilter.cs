namespace ArtemVasilevKt_42_23.Filters;

public class StudentFilter
{
    public string? GroupName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool? IsDeleted { get; set; }
}

public class GroupsWithFailsFilter
{
    public string DisciplineName { get; set; } = string.Empty;
}