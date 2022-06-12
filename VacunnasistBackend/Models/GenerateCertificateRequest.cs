using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class GenerateCertificateRequest
    {
        [Required]
        public int Id { get; set; }
    }
}