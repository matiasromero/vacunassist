using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacunnasistBackend.Models;

namespace VacunassistBackend.Data
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(b => b.Email).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
        }
    }
}
