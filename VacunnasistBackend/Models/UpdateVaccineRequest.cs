namespace VacunassistBackend.Models
{
    public class UpdateVaccineRequest
    {
        public string? Name { get; set; }
        public bool? CanBeRequested { get; set; }
        public bool? IsActive { get; set; }
    }
}