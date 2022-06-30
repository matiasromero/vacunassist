namespace VacunassistBackend.Models.Filters
{
    public class VaccinesFilterRequest
    {

        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? CanBeRequested { get; set; }
    }
}