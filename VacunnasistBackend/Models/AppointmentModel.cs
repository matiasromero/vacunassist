using VacunassistBackend.Entities;

namespace VacunassistBackend.Models
{
    public class AppointmentModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int VaccineId { get; set; }
        public string VaccineName { get; set; }
        public AppointmentStatus Status { get; set; }
        public bool Notified { get; set; }
        public DateTime RequestedAt { get; set; }

        public int? VacinatorId { get; set; }
        public string? VacinatorName { get; set; }

        public int? PreferedOfficeId { get; set; }
        public string? PreferedOfficeName { get; set; }
        public string? PreferedOfficeAddress { get; set; }

        public string? Comment { get; set; }
        public DateTime? AppliedDate { get; set; }
    }
}