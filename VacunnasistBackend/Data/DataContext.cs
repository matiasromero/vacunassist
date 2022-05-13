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

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);

            if (bool.Parse(Configuration.GetValue<String>("SeedDatabase", "false")))
            {
                var admin = new User
                {
                    Id = 1,
                    UserName = "Admin",
                    Role = UserRoles.Administrator,
                    Address = "Calle Falsa 1234, La Plata",
                    BelongsToRiskGroup = false,
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234")
                };

                var vacunador1 = new User
                {
                    Id = 2,
                    UserName = "Vacunador",
                    Role = UserRoles.Vacunator,
                    Address = "Calle Falsa 4567, La Plata",
                    BelongsToRiskGroup = false,
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234")
                };

                var patient1 = new User
                {
                    Id = 3,
                    UserName = "Paciente",
                    Role = UserRoles.Patient,
                    Address = "Calle Falsa 789, La Plata",
                    BelongsToRiskGroup = false,
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234")
                };

                modelBuilder.Entity<User>().HasData(admin, vacunador1, patient1);
            }
        }
        #endregion
    }
}
