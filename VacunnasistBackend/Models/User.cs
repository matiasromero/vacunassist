namespace VacunnasistBackend.Models;

public class User
{
    public User()
    {
    }

    public User(string email)
        : this()
    {
        Email = email;
    }

    public int Id { get; set; }

    public string? PasswordHash { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}