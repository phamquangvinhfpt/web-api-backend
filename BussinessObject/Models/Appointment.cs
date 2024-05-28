using BussinessObject.Enums;

namespace BussinessObject.Models
{
    public class Appointment : BaseEntity
    {
    public Guid PatientID { get; set; }
    public Guid DentistID { get; set; }
    public Guid ClinicID { get; set; }
    public TimeSpan TimeSlot { get; set; }
    public AppointmentType AppointmentType { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public DateTime Date { get; set; }
    public required AppUser Patient { get; set; }
    public required AppUser Dentist { get; set; }
    public Clinic Clinic { get; set; }
    public DentalRecord DentalRecord { get; set; }
    }
}