namespace ArtemVasilevKt_42_23.Dto;

public class StudentDto
{
    public int StudentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}