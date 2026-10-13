using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ArtemVasilevKt_42_23.Helpers;
using ArtemVasilevKt_42_23.Models;
namespace ArtemVasilevKt_42_23.Configurations
{
    public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
    {
        public void Configure(EntityTypeBuilder<Discipline> builder)
        {
            builder.ToTable("Disciplines");
            builder.HasKey(d => d.DisciplineId);

            builder.Property(d => d.Name).IsRequired().HasColumnType(ColumnType.NVarchar200);
            builder.Property(d => d.IsDeleted).IsRequired().HasColumnType(ColumnType.Bit).HasDefaultValue(false);
        }
    }
}
