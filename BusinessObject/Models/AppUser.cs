using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BusinessObject.Enums;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public UserStatus Status { get; set; }
        public required string FullName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public virtual List<Appointment> Appointments { get; set; }
        [JsonIgnore]
        public virtual List<Message> SentMessages { get; set; }
        [JsonIgnore]
        public virtual List<Message> ReceivedMessages { get; set; }
        [JsonIgnore]
        public virtual DentistDetail DentistDetails { get; set; }
    }
}