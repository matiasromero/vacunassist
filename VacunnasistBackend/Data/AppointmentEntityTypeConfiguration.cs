using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacunassistBackend.Entities;

namespace VacunassistBackend.Data
{
    public class AppointmentEntityTypeConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasOne(b => b.Patient).WithMany().IsRequired();
            builder.HasOne(b => b.Vaccine).WithMany().IsRequired();
            builder.HasOne(b => b.PreferedOffice).WithMany();
            builder.Property(b => b.Comment).HasMaxLength(200);
        }
    }
}
