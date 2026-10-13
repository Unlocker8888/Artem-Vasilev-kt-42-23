using System.Diagnostics;

namespace ArtemVasilevKt_42_23.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsDeleted { get; set; }

        public Group Group { get; set; } = null!;
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();


    }
}
