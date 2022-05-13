using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nombre de usuario es requerido")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]
        public string? Password { get; set; }
    }
}