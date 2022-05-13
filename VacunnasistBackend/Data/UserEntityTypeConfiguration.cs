using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacunassistBackend.Models;

namespace VacunassistBackend.Data
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(b => b.UserName).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Address).HasMaxLength(200).IsRequired();
        }
    }
}
