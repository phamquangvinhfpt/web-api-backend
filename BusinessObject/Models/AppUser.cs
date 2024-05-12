using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public UserStatus Status { get; set; }
        public required string FullName { get; set; }
        public DateTime? DOB { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public byte[]? AvatarImage { get; set; }
        [NotMapped]
        public string? AvatarImageBase64 {
            get {
                if (AvatarImage == null) return null;
                return "data:image/png;base64," + Convert.ToBase64String(AvatarImage);
            }
        }
        public virtual List<Appointment> Appointments { get; set; }
        public virtual List<Message> SentMessages { get; set; }
        public virtual List<Message> ReceivedMessages { get; set; }
        public virtual DentistDetail DentistDetails { get; set; }
    }
}