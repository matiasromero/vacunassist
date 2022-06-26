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
        private INotificationsService _notificationsService;

        public AppointmentsService(DataContext context, INotificationsService notificationsService)
        {
            this._context = context;
            this._notificationsService = notificationsService;
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
            var shouldNotify = false;
            var appointment = _context.Appointments.Include(x => x.Patient).Include(x => x.Vaccine).Include(x => x.Vaccinator).FirstOrDefault(x => x.Id == id);
            if (appointment == null)
                throw new HttpResponseException(400, message: "Solicitud no encontrada");

            if (request.Status.HasValue && request.Status != appointment.Status)
            {
                if ((appointment.Status == AppointmentStatus.Confirmed || appointment.Status == AppointmentStatus.Pending)
                && request.Status.Value == AppointmentStatus.Cancelled)
                {
                    appointment.Status = request.Status.Value;
                    _notificationsService.SendCancellation(appointment);
                }
                if (appointment.Status == AppointmentStatus.Confirmed
                && request.Status.Value == AppointmentStatus.Done)
                {
                    appointment.Status = request.Status.Value;
                    var patient = appointment.Patient;
                    var newApplied = new AppliedVaccine();
                    newApplied.AppliedBy = appointment.Vaccinator!.FullName;
                    newApplied.AppliedDate = DateTime.Now;
                    newApplied.AppointmentId = appointment.Id;
                    newApplied.Comment = request.Comment;
                    newApplied.UserId = appointment.Patient.Id;
                    newApplied.VaccineId = appointment.Vaccine.Id;
                    patient.Vaccines.Add(newApplied);
                }
                appointment.Status = request.Status.Value;
            }

            if (string.IsNullOrEmpty(request.Comment) == false)
            {
                appointment.Comment = request.Comment;
            }

            if (request.Date.HasValue && request.Date != appointment.Date)
            {
                appointment.Date = request.Date.Value;
                appointment.Notified = false;
                shouldNotify = true;
            }

            if (request.OfficeId.HasValue && appointment.PreferedOffice != null && request.OfficeId != appointment.PreferedOffice.Id)
            {
                appointment.PreferedOffice = this._context.Offices.First(x => x.Id == request.OfficeId);
                appointment.Notified = false;
                shouldNotify = true;
            }

            if (request.VaccinatorId.HasValue && appointment.Vaccinator != null && request.VaccinatorId != appointment.Vaccinator.Id)
            {
                appointment.Vaccinator = this._context.Users.First(x => x.Id == request.VaccinatorId);
                appointment.Notified = false;
                shouldNotify = true;
            }

            if (request.VaccineId.HasValue && appointment.Vaccine != null && request.VaccineId != appointment.Vaccine.Id)
            {
                var exist = AlreadyExist(appointment.Patient.Id, request.VaccineId.Value);
                if (exist)
                    throw new HttpResponseException(400, message: "El paciente ya tiene una vacuna pendiente para esta vacuna.");
                appointment.Vaccine = this._context.Vaccines.First(x => x.Id == request.VaccineId);
                appointment.Notified = false;
                shouldNotify = true;
            }

            _context.SaveChanges();

            if (shouldNotify)
                _notificationsService.Trigger(appointment.Id);
        }
    }
}