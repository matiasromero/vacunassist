using VacunassistBackend.Entities;
using VacunassistBackend.Infrastructure;
using VacunassistBackend.Models;
using VacunassistBackend.Models.Filters;

namespace VacunassistBackend.Services
{
    public interface IOfficesService
    {
        Office[] GetAll(OfficesFilterRequest filter);
        Office Get(int id);
        void Update(int id, UpdateOfficeRequest model);
        bool CanBeDeleted(int id);
        bool Exist(int id);
        bool AlreadyExist(string name);
        bool New(NewOfficeRequest model);
    }

    public class OfficesService : IOfficesService
    {
        private DataContext _context;

        public OfficesService(DataContext context)
        {
            this._context = context;
        }

        public Office[] GetAll(OfficesFilterRequest filter)
        {
            var query = _context.Offices.AsQueryable();
            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);
            if (string.IsNullOrEmpty(filter.Name) == false)
                query = query.Where(x => x.Name.Contains(filter.Name));
            return query.ToArray();
        }

        public bool New(NewOfficeRequest model)
        {
            // validate
            if (_context.Offices.Any(x => x.Name == model.Name))
                throw new ApplicationException("Nombre de sede '" + model.Name + "' en uso");

            try
            {
                var office = new Office()
                {
                    Name = model.Name
                };

                // save vaccine
                _context.Offices.Add(office);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Office Get(int id)
        {
            return _context.Offices.First(x => x.Id == id);
        }

        public void Update(int id, UpdateOfficeRequest model)
        {
            var office = _context.Offices.FirstOrDefault(x => x.Id == id);
            if (office == null)
                throw new HttpResponseException(400, message: "Sede no encontrada");

            if (string.IsNullOrEmpty(model.Name) == false && model.Name != office.Name)
            {
                var existOther = _context.Vaccines.Any(x => x.Name == model.Name && x.Id != id);
                if (existOther)
                {
                    throw new HttpResponseException(400, message: "Nombre de sede '" + model.Name + "' en uso");
                }
                office.Name = model.Name;

            }
            if (model.IsActive.HasValue && model.IsActive != office.IsActive)
            {
                office.IsActive = model.IsActive.Value;
            }

            _context.SaveChanges();
        }

        private static void CheckIfExists(Office? office)
        {
            if (office == null)
                throw new HttpResponseException(400, "Sede no encontrada");
        }

        public bool CanBeDeleted(int id)
        {
            var office = _context.Offices.FirstOrDefault(x => x.Id == id);
            CheckIfExists(office);
            var appointments = _context.Appointments
            .Where(x => x.PreferedOffice != null && x.PreferedOffice.Id == office.Id &&
            (x.Status == AppointmentStatus.Pending || x.Status == AppointmentStatus.Confirmed))
            .ToArray();
            return appointments.Any() == false;
        }

        public bool Exist(int id)
        {
            return _context.Offices.Any(x => x.Id == id);
        }

        public bool AlreadyExist(string name)
        {
            return _context.Offices.Any(x => x.Name == name);
        }
    }
}