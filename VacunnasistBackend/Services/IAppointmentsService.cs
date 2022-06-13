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