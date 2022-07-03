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
        public DbSet<Office> Offices { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);

            if (bool.Parse(Configuration.GetValue<String>("SeedDatabase", "false")))
            {
                var office1 = new Office
                {
                    Id = 1,
                    Name = "La Plata I",
                    Address = "Calle 52 113, La Plata",
                    IsActive = true,
                };
                var office2 = new Office
                {
                    Id = 2,
                    Name = "Quilmes",
                    Address = "Calle Falsa 100, La Plata",
                    IsActive = true,
                };
                var office3 = new Office
                {
                    Id = 3,
                    Name = "La Plata II",
                    Address = "Calle 14 1140, La Plata",
                    IsActive = true,
                };

                var admin = new User
                {
                    Id = 1,
                    UserName = "Admin",
                    Role = UserRoles.Administrator,
                    Address = "Calle Falsa 1234, La Plata",
                    FullName = "Administrador",
                    BirthDate = DateTime.Now.Date,
                    DNI = "11111111",
                    Gender = Gender.Other,
                    PhoneNumber = "2215897845",
                    Email = "admin@vacunassist.com",
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
                    DNI = "11111111",
                    Gender = Gender.Other,
                    PhoneNumber = "1158987895",
                    Email = "vacunador@email.com",
                    BelongsToRiskGroup = false,
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234"),
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
                    BirthDate = new DateTime(1987, 07, 06),
                    DNI = "12548987",
                    Gender = Gender.Other,
                    BelongsToRiskGroup = false,
                    IsActive = true,
                    PasswordHash = PasswordHash.CreateHash("1234"),
                    PreferedOfficeId = office1.Id
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
                    PasswordHash = PasswordHash.CreateHash("1234"),
                    PreferedOfficeId = office2.Id
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
                    IsActive = true,
                    CanBeRequested = false
                };
                var vaccine3 = new Vaccine
                {
                    Id = 3,
                    Name = "Gripe",
                    IsActive = true
                };

                var applied1 = new AppliedVaccine()
                {
                    Id = 1,
                    UserId = patient1.Id,
                    VaccineId = vaccine1.Id,
                    AppliedDate = new DateTime(2022, 03, 12, 10, 30, 01)
                };
                var applied2 = new AppliedVaccine()
                {
                    Id = 2,
                    UserId = patient1.Id,
                    VaccineId = vaccine2.Id,
                    AppliedDate = new DateTime(2022, 05, 10, 14, 30, 25)
                };
                var applied3 = new AppliedVaccine()
                {
                    Id = 3,
                    UserId = patient1.Id,
                    VaccineId = vaccine3.Id,
                };

                modelBuilder.Entity<Office>().HasData(office1, office2, office3);
                modelBuilder.Entity<Vaccine>().HasData(vaccine1, vaccine2, vaccine3);
                modelBuilder.Entity<User>().HasData(admin, vacunador1, patient1, patient2);
                modelBuilder.Entity<AppliedVaccine>().HasData(applied1, applied2, applied3);
            }
        }
        #endregion
    }
}
