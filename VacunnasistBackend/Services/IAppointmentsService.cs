using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Entities;
using VacunassistBackend.Infrastructure;
using VacunassistBackend.Models;
using VacunassistBackend.Models.Filters;
using VacunassistBackend.Utils;

namespace VacunassistBackend.Services
{
    public interface IAppointmentsService
    {
        bool AlreadyExist(int userId, int vaccineId);
        void Add(int userId, int vaccineId);
        bool Exist(int appointmentId);
        void Update(int id, UpdateAppointmentRequest request);
        Appointment[] GetAll(AppointmentsFilterRequest filter);
        Appointment Get(int id);
        void AddConfirmed(NewConfirmedAppointmentRequest model);
    }

    public class AppointmentsService : IAppointmentsService
    {
        private DataContext _context;

        public AppointmentsService(DataContext context)
        {
            this._context = context;
        }

        public void Add(int userId, int vaccineId)
        {
            var user = _context.Users.First(x => x.Id == userId);
            if (user.Role != UserRoles.Patient)
            {
                throw new ApplicationException("El usuario no es un paciente");
            }

            var vaccine = this._context.Vaccines.First(x => x.Id == vaccineId);
            var appointment = new Appointment(user, vaccine)
            {
                RequestedAt = DateTime.Now
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public void AddConfirmed(NewConfirmedAppointmentRequest model)
        {
            var user = _context.Users.First(x => x.Id == model.PatientId);
            if (user.Role != UserRoles.Patient)
            {
                throw new ApplicationException("El usuario no es un paciente");
            }

            var vaccine = this._context.Vaccines.First(x => x.Id == model.VaccineId);
            var vaccinator = this._context.Users.First(x => x.Id == model.VaccinatorId);
            var office = this._context.Offices.First(x => x.Id == model.OfficeId);
            var appointment = model.CurrentId.HasValue ?
            this._context.Appointments.First(x => x.Id == model.CurrentId.Value)
            : new Appointment(user, vaccine);
            appointment.Vaccine = vaccine;
            appointment.RequestedAt = DateTime.Now;
            appointment.Date = model.Date;
            appointment.PreferedOffice = office;
            appointment.Vaccinator = vaccinator;
            appointment.Status = AppointmentStatus.Confirmed;

            if (model.CurrentId.HasValue == false)
                _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public bool AlreadyExist(int userId, int vaccineId)
        {
            var vaccine = _context.Vaccines.First(x => x.Id == vaccineId);
            var user = _context.Users.First(x => x.Id == userId);
            return _context.Appointments.Any(x => x.Patient.Id == userId && x.Vaccine.Id == vaccineId
            && (x.Status == AppointmentStatus.Confirmed || x.Status == AppointmentStatus.Pending));
        }

        public bool Exist(int appointmentId)
        {
            return _context.Appointments.Any(x => x.Id == appointmentId);
        }

        public Appointment Get(int id)
        {
            var query = _context.Appointments.Include(u => u.Patient).Include(x => x.PreferedOffice).Include(x => x.Vaccinator).Include(x => x.Vaccine).AsQueryable();
            var result = query.First(x => x.Id == id);
            if (result.PreferedOffice == null)
            {
                result.PreferedOffice = result.Patient.PreferedOffice;
            }
            return result;
        }

        public Appointment[] GetAll(AppointmentsFilterRequest filter)
        {
            var query = _context.Appointments.Include(u => u.Patient).Include(x => x.PreferedOffice).Include(x => x.Vaccinator).Include(x => x.Vaccine).AsQueryable();
            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status);
            if (filter.Date.HasValue)
                query = query.Where(x => x.RequestedAt.Date == filter.Date.Value.Date);
            if (string.IsNullOrEmpty(filter.FullName) == false)
                query = query.Where(x => x.Patient.FullName.Contains(filter.FullName));
            if (filter.OfficeId.HasValue)
                query = query.Where(x => x.PreferedOffice.Id == filter.OfficeId.Value);
            if (filter.VaccinatorId.HasValue)
                query = query.Where(x => x.Vaccinator.Id == filter.VaccinatorId.Value);
            return query.ToArray();
        }

        public void Update(int id, UpdateAppointmentRequest request)
        {
            var appointment = _context.Appointments.Include(x => x.Patient).Include(x => x.Vaccine).Include(x => x.Vaccinator).FirstOrDefault(x => x.Id == id);
            if (appointment == null)
                throw new HttpResponseException(400, message: "Solicitud no encontrada");

            if (request.Status.HasValue && request.Status != appointment.Status)
            {
                appointment.Status = request.Status.Value;
            }

            _context.SaveChanges();
        }
    }
}