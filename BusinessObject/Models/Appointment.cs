using BusinessObject.Enums;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public class Appointment : BaseEntity
    {
        public Guid PatientID { get; set; }
        public Guid DentistID { get; set; }
        public Guid ClinicID { get; set; }
        public TimeSpan TimeSlot { get; set; }
        public AppointmentType Type { get; set; }
        public int duration { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime Date { get; set; }
        public AppUser Patient { get; set; }
        public AppUser Dentist { get; set; }
        public Clinic Clinic { get; set; }
        [JsonIgnore]
        public DentalRecord DentalRecord { get; set; }
    }
}