using VacunassistBackend.Entities;

namespace VacunassistBackend.Models.Filters
{
    public class AppointmentsFilterRequest
    {
        public string? FullName { get; set; }
        public DateTime? Date { get; set; }
        public AppointmentStatus? Status { get; set; }
        public int? VaccinatorId { get; set; }
        public int? OfficeId { get; set; }
    }
}