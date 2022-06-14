using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class NewAppointmentRequest
    {
        [Required]
        public int VaccineId { get; set; }
    }
}