using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ArtemVasilevKt_42_23.Helpers;
using ArtemVasilevKt_42_23.Models;

namespace ArtemVasilevKt_42_23.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Groups");
            builder.HasKey(g => g.GroupId);

            builder.Property(g => g.Name).IsRequired().HasColumnType(ColumnType.NVarchar50);
            builder.Property(g => g.Course).IsRequired().HasColumnType(ColumnType.Int);
            builder.Property(g => g.IsDeleted).IsRequired().HasColumnType(ColumnType.Bit).HasDefaultValue(false);

            builder.HasOne(g => g.Speciality)
                   .WithMany(s => s.Groups)
                   .HasForeignKey(g => g.SpecialityId)
                   .HasConstraintName("FK_Groups_Specialities_SpecialityId")
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
