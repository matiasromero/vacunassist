using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Entities;
using VacunassistBackend.Infrastructure;
using VacunassistBackend.Models;
using VacunassistBackend.Models.Filters;

namespace VacunassistBackend.Services
{
    public interface IVaccinesService
    {
        Vaccine[] GetAll(VaccinesFilterRequest filter);
        Vaccine Get(int id);
        bool ExistsApplied(int id);
        void Update(int id, UpdateVaccineRequest model);
        bool CanBeDeleted(int id);
        bool Exist(int id);
        bool New(NewVaccineRequest model);
        AppliedVaccine GetApplied(int id);
        AppliedVaccine GetAppliedByAppointment(int id);
    }

    public class VaccinesService : IVaccinesService
    {
        private DataContext _context;

        public VaccinesService(DataContext context)
        {
            this._context = context;
        }

        public bool ExistsApplied(int id)
        {
            return _context.AppliedVaccines.Any(x => x.Id == id);
        }

        public Vaccine[] GetAll(VaccinesFilterRequest filter)
        {
            var query = _context.Vaccines.AsQueryable();
            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);
            if (filter.CanBeRequested.HasValue)
                query = query.Where(x => x.CanBeRequested == filter.CanBeRequested);
            if (string.IsNullOrEmpty(filter.Name) == false)
                query = query.Where(x => x.Name.Contains(filter.Name));
            return query.ToArray();
        }

        public bool New(NewVaccineRequest model)
        {
            // validate
            if (_context.Vaccines.Any(x => x.Name == model.Name))
                throw new ApplicationException("Nombre de vacuna '" + model.Name + "' en uso");

            try
            {
                var vaccine = new Vaccine()
                {
                    Name = model.Name,
                    CanBeRequested = model.CanBeRequested
                };

                // save vaccine
                _context.Vaccines.Add(vaccine);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AppliedVaccine GetApplied(int id)
        {
            return _context.AppliedVaccines.Include(u => u.Vaccine).First(x => x.Id == id);
        }

        public AppliedVaccine GetAppliedByAppointment(int id)
        {
            return _context.AppliedVaccines.Include(u => u.Vaccine).First(x => x.Appointment != null && x.Appointment.Id == id);
        }

        public Vaccine Get(int id)
        {
            return _context.Vaccines.First(x => x.Id == id);
        }

        public void Update(int id, UpdateVaccineRequest model)
        {
            var user = _context.Vaccines.FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new HttpResponseException(400, message: "Vacuna no encontrada");

            if (string.IsNullOrEmpty(model.Name) == false && model.Name != user.Name)
            {
                var existOther = _context.Vaccines.Any(x => x.Name == model.Name && x.Id != id);
                if (existOther)
                {
                    throw new HttpResponseException(400, message: "Nombre de vacuna '" + model.Name + "' en uso");
                }
                user.Name = model.Name;

            }
            if (model.CanBeRequested.HasValue && model.CanBeRequested != user.CanBeRequested)
            {
                user.CanBeRequested = model.CanBeRequested.Value;
            }
            if (model.IsActive.HasValue && model.IsActive != user.IsActive)
            {
                user.IsActive = model.IsActive.Value;
            }

            _context.SaveChanges();
        }

        private static void CheckIfExists(Vaccine? vaccine)
        {
            if (vaccine == null)
                throw new HttpResponseException(400, "Vacuna no encontrada");
        }

        public bool CanBeDeleted(int id)
        {
            var vaccine = _context.Vaccines.FirstOrDefault(x => x.Id == id);
            CheckIfExists(vaccine);
            var appointments = _context.Appointments
            .Where(x => x.Vaccine.Id == vaccine.Id &&
            (x.Status == AppointmentStatus.Pending || x.Status == AppointmentStatus.Confirmed))
            .ToArray();
            return appointments.Any() == false;
        }

        public bool Exist(int id)
        {
            return _context.Vaccines.Any(x => x.Id == id);
        }
    }
}