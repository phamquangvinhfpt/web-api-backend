using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public class Clinic : BaseEntity
    {
        public Guid OwnerID { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }
        public bool Verified { get; set; }
        public AppUser Owner { get; set; }
        public List<ClinicDetail> ClinicDetails { get; set; }
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }
    }
}