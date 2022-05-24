using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Entities;
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
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<AppliedVaccine> AppliedVaccines { get; set; }

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
                    FullName = "Administrador",
                    BirthDate = DateTime.Now.Date,
                    DNI = string.Empty,
                    Gender = Gender.Other,
                    PhoneNumber = "",
                    Email = "",
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
                    FullName = "Vacunador",
                    BirthDate = DateTime.Now.Date,
                    DNI = string.Empty,
                    Gender = Gender.Other,
                    PhoneNumber = "",
                    Email = "",
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
                    FullName = "Paciente",
                    PhoneNumber = "11-8795-1478",
                    Email = "email@email.com",
                    BirthDate = DateTime.Now.Date,
                    DNI = string.Empty,
                    Gender = Gender.Other,
                    BelongsToRiskGroup = false,
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234"),
                };

                var patient2 = new User
                {
                    Id = 4,
                    UserName = "jperez",
                    Role = UserRoles.Patient,
                    Address = "Calle Falsa 111, La Plata",
                    FullName = "Juan Perez",
                    PhoneNumber = "211-235-1478",
                    Email = "email2@email.com",
                    BirthDate = new DateTime(1987, 06, 07).Date,
                    DNI = "33170336",
                    Gender = Gender.Male,
                    BelongsToRiskGroup = false,
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234")
                };

                var vaccine1 = new Vaccine
                {
                    Id = 1,
                    Name = "COVID-19",
                    IsActive = true
                };
                var vaccine2 = new Vaccine
                {
                    Id = 2,
                    Name = "Fiebre amarilla",
                    IsActive = true
                };
                var vaccine3 = new Vaccine
                {
                    Id = 3,
                    Name = "Gripe",
                    IsActive = true
                };

                var applied1 = new AppliedVaccine()
                {
                    UserId = patient1.Id,
                    VaccineId = vaccine1.Id,
                    AppliedDate = new DateTime(2022, 03, 12, 10, 30, 01)
                };
                var applied2 = new AppliedVaccine()
                {
                    UserId = patient1.Id,
                    VaccineId = vaccine2.Id,
                    AppliedDate = new DateTime(2022, 05, 10, 14, 30, 25)
                };

                modelBuilder.Entity<Vaccine>().HasData(vaccine1, vaccine2, vaccine3);
                modelBuilder.Entity<User>().HasData(admin, vacunador1, patient1, patient2);
                modelBuilder.Entity<AppliedVaccine>().HasData(applied1, applied2);
            }
        }
        #endregion
    }
}
