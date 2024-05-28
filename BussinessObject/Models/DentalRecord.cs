namespace BussinessObject.Models
{
    public class DentalRecord : BaseEntity
    {
        public Guid AppointmentID { get; set; }
        public required string TreatmentDetails { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public required Appointment Appointment { get; set; }
    }
}