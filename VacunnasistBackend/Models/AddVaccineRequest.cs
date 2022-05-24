using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class AddVaccineRequest
    {
        [Required]
        public int VaccineId { get; set; }

        public string? Comment { get; set; }
        public DateTime? AppliedDate { get; set; }
    }
}