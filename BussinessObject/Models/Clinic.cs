namespace BussinessObject.Models
{
    public class Clinic : BaseEntity
    {
        public Guid OwnerID { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }
        public bool Verified { get; set; }
        public required AppUser Owner { get; set; }
        public required List<ClinicOpeningHour> OpeningHours { get; set; }
        public required List<Appointment> Appointments { get; set; }
    }
}