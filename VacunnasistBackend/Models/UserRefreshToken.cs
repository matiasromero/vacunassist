using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunnasistBackend.Models;

public class UserRefreshToken
{
    public User User { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime? Expiration { get; set; }
}