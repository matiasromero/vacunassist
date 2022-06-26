namespace VacunassistBackend.Entities;

public class User
{
    public User()
    {
        IsActive = true;
        Vaccines = new List<AppliedVaccine>();
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
    public int Age
    {
        get
        {
            var now = DateTime.Today;
            int age = now.Year - BirthDate.Year;
            if (now < BirthDate.AddYears(age))
                age--;
            return age;
        }
    }
    public string FullName { get; set; } = string.Empty;

    public string DNI { get; set; }

    public string Gender { get; set; }

    public bool BelongsToRiskGroup { get; set; } = false;

    public string Role { get; set; } = string.Empty;

    public int? PreferedOfficeId { get; set; }
    public Office? PreferedOffice { get; set; }

    public bool IsActive { get; set; }
    public virtual List<AppliedVaccine> Vaccines { get; set; }

    public virtual int GetAge()
    {
        int Age = (int)(DateTime.Today - BirthDate).TotalDays;
        Age = Age / 365;
        return Age;
    }
}

public static class UserRoles
{
    public static string Administrator = "administrator";
    public static string Vacunator = "vacunator";
    public static string Patient = "patient";
}

public static class Gender
{
    public static string Male = "male";
    public static string Female = "female";
    public static string Other = "other";
}