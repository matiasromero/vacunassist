namespace VacunassistBackend.Models
{
    public class UpdateOfficeRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
    }
}