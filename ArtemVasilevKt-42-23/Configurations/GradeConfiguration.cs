using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ArtemVasilevKt_42_23.Helpers;
using ArtemVasilevKt_42_23.Models;

namespace ArtemVasilevKt_42_23.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grades");
            builder.HasKey(g => g.GradeId);

            builder.Property(g => g.Value).IsRequired().HasColumnType(ColumnType.Int);

            builder.HasOne(g => g.Student)
                   .WithMany(s => s.Grades)
                   .HasForeignKey(g => g.StudentId)
                   .HasConstraintName("FK_Grades_Students_StudentId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.Discipline)
                   .WithMany(d => d.Grades)
                   .HasForeignKey(g => g.DisciplineId)
                   .HasConstraintName("FK_Grades_Disciplines_DisciplineId")
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
