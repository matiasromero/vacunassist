using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacunassistBackend.Entities;

namespace VacunassistBackend.Data
{
    public class AppliedVaccineEntityTypeConfiguration : IEntityTypeConfiguration<AppliedVaccine>
    {
        public void Configure(EntityTypeBuilder<AppliedVaccine> builder)
        {
            builder.HasOne(av => av.User).WithMany(u => u.Vaccines).HasForeignKey(av => av.UserId).IsRequired();
            builder.HasOne(av => av.Vaccine).WithMany(u => u.Users).HasForeignKey(av => av.VaccineId).IsRequired();
        }
    }
}
