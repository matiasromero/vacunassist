using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}