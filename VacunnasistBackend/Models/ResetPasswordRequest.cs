using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        public string UserName { get; set; }
    }
}