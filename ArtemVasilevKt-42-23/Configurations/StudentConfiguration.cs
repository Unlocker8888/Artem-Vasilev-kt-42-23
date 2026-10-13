using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ArtemVasilevKt_42_23.Models;
using ArtemVasilevKt_42_23.Helpers;


namespace ArtemVasilevKt_42_23.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(s => s.StudentId);

            builder.Property(s => s.FirstName).IsRequired().HasColumnType(ColumnType.NVarchar100);
            builder.Property(s => s.LastName).IsRequired().HasColumnType(ColumnType.NVarchar100);
            builder.Property(s => s.IsDeleted).IsRequired().HasColumnType(ColumnType.Bit).HasDefaultValue(false);

            builder.HasOne(s => s.Group)
                   .WithMany(g => g.Students)
                   .HasForeignKey(s => s.GroupId)
                   .HasConstraintName("FK_Students_Groups_GroupId")
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
