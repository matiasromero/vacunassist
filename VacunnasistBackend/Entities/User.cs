namespace VacunnasistBackend.Entities;

public class User
{
    public User()
    {
        IsActive = true;
    }

    public User(string username)
        : this()
    {
        UserName = username;
    }

    public int Id { get; set; }

    public string? PasswordHash { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
    public string FullName { get; set; } = string.Empty;

    public string DNI { get; set; }

    public string Gender { get; set; }

    public bool BelongsToRiskGroup { get; set; } = false;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public static class UserRoles
{
    public static string Administrator = "administrator";
    public static string Vacunator = "vacunator";
    public static string Patient = "patient";
}

public static class Gender
{
    public static string Male = "hombre";
    public static string Female = "mujer";
    public static string Other = "otro";
}