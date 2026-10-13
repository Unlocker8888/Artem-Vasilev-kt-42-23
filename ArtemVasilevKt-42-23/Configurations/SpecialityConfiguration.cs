using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ArtemVasilevKt_42_23.Helpers;
using ArtemVasilevKt_42_23.Models;
namespace ArtemVasilevKt_42_23.Configurations
{
    public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
    {
        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.ToTable("Specialities");
            builder.HasKey(s => s.SpecialityId);

            builder.Property(s => s.Title).IsRequired().HasColumnType(ColumnType.NVarchar200);
            builder.Property(s => s.Code).IsRequired().HasColumnType(ColumnType.NVarchar50);
            builder.HasIndex(s => s.Code).IsUnique();
        }
    }
}
