using System.ComponentModel.DataAnnotations;
using VacunassistBackend.Entities;

namespace VacunassistBackend.Models
{
    public class UpdateAppointmentRequest
    {
        public AppointmentStatus? Status { get; set; }
    }
}