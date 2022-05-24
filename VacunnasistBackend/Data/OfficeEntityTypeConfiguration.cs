using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacunassistBackend.Entities;

namespace VacunassistBackend.Data
{
    public class OfficeEntityTypeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Address).HasMaxLength(200).IsRequired();
        }
    }
}
