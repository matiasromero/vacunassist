namespace VacunassistBackend.Models;

public class User
{
    public User()
    {
    }

    public User(string username)
        : this()
    {
        UserName = username;
    }

    public int Id { get; set; }

    public string? PasswordHash { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public bool BelongsToRiskGroup { get; set; } = false;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}