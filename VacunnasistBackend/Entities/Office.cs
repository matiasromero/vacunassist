namespace VacunassistBackend.Entities;

public class Office
{
    public Office()
    {
        IsActive = true;
    }

    public Office(string name)
        : this()
    {
        Name = name;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}