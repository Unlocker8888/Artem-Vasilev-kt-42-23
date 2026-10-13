namespace ArtemVasilevKt_42_23.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Course { get; set; }
        public int SpecialityId { get; set; }
        public bool IsDeleted { get; set; }

        public Speciality Speciality { get; set; } = null!;
        public ICollection<Student> Students { get; set; } = new List<Student>();


    }
}
