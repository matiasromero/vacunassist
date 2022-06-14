using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class NewConfirmedAppointmentRequest
    {
        [Required]
        public int VaccineId { get; set; }
        [Required]
        public int VaccinatorId { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int OfficeId { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}