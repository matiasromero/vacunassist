using System.ComponentModel.DataAnnotations;

namespace VacunassistBackend.Models
{
    public class NotifyUsersRequest
    {
        [Required]
        public string Comment { get; set; }
    }
}