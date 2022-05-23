using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class UpdateUserRequest
    {
        public string? Password { get; set; }
        public string? FullName { get; set; }

        public string? Address { get; set; }

        public bool? BelongsToRiskGroup { get; set; }

        public string? Gender { get; set; }

        public string? Email { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? DNI { get; set; }

        public string? PhoneNumber { get; set; }
    }
}