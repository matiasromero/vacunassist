namespace VacunassistBackend.Entities;
public class Appointment
{
    public Appointment()
    {
        Status = AppointmentStatus.Pending;
    }

    public Appointment(User user, Vaccine vaccine)
        : this()
    {
        Patient = user;
        Vaccine = vaccine;
        Status = AppointmentStatus.Pending;
    }

    public int Id { get; set; }
    public AppointmentStatus Status { get; set; }
    public User Patient { get; set; }
    public Vaccine Vaccine { get; set; }
    public DateTime RequestedAt { get; set; }
    public bool Notified { get; set; }
    public Office? PreferedOffice { get; set; }
    public string? Comment { get; set; }

    public DateTime? Date { get; set; }
    public User? Vaccinator { get; set; }

    public DateTime? AppliedDate { get; set; }
}

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Done,
    Cancelled
}
