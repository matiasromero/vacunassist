using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacunassistBackend.Models;

namespace VacunassistBackend.Data
{
    public class UserRefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.HasOne(b => b.User).WithOne().HasForeignKey<User>(u => u.Id);
            builder.HasNoKey();
        }
    }
}
