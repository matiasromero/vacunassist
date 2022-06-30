namespace VacunassistBackend.Models
{
    public class NewVaccineRequest
    {
        public string Name { get; set; }
        public bool CanBeRequested { get; set; }
    }
}