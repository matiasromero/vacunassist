using VacunassistBackend.Entities;

namespace VacunassistBackend.Models
{
    public class UpdateAppointmentRequest
    {
        public AppointmentStatus? Status { get; set; }
        public DateTime? Date { get; set; }
        public int? VaccinatorId { get; set; }
        public int? OfficeId { get; set; }
        public int? VaccineId { get; set; }
        public string? Comment { get; set; }
    }
}