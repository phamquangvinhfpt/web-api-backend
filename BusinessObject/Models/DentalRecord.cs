namespace BusinessObject.Models
{
    public class DentalRecord : BaseEntity
    {
        public Guid AppointmentID { get; set; }
        public required string TreatmentDetails { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Appointment Appointment { get; set; }
        public List<Prescription> Prescriptions { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public List<FollowUpAppointment> FollowUpAppointments { get; set; }
    }
}