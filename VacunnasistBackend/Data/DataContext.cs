using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Models;
using VacunassistBackend.Utils;

namespace VacunassistBackend.Data
{
    public class DataContext : DbContext
    {
        private IConfiguration Configuration { get; }

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRefreshToken> RefreshTokens { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);

            if (bool.Parse(Configuration.GetValue<String>("SeedDatabase", "false")))
            {
                modelBuilder.Entity<User>().HasData(new User
                {
                    Id = 1,
                    Email = "admin@mail.com",
                    Name = "Admin",
                    Role = "administrator",
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234")
                });
            }
        }
        #endregion
    }
}
