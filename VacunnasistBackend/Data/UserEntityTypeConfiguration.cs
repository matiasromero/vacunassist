using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacunassistBackend.Entities;

namespace VacunassistBackend.Data
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(b => b.UserName).HasMaxLength(20).IsRequired();
            builder.Property(b => b.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(b => b.Address).HasMaxLength(200).IsRequired();
            builder.Property(b => b.DNI).HasMaxLength(20).IsRequired();
            builder.Property(b => b.Email).HasMaxLength(100).IsRequired();
            builder.Property(b => b.FullName).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Gender).HasMaxLength(50).IsRequired();
        }
    }
}
