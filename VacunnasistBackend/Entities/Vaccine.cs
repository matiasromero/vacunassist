namespace VacunassistBackend.Entities;

public class Vaccine
{
    public Vaccine()
    {
        IsActive = true;
    }

    public Vaccine(string name)
        : this()
    {
        Name = name;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool CanBeRequested { get; set; } = true;
    public virtual List<AppliedVaccine> Users { get; set; }
}