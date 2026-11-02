namespace ArtemVasilevKt_42_23.Models
{
    public enum DisciplineDirection
    {
        Humanitarian = 0,
        Technical = 1
    }

    public class Discipline
    {
        public int DisciplineId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DisciplineDirection Direction { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
