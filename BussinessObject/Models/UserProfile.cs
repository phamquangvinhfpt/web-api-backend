using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessObject.Enums;

namespace BussinessObject.Models
{
    public class UserProfile : BaseEntity
    {
        public Guid UserProfileID { get; set; }
        public Guid UserID { get; set; }
        public required string FullName { get; set; }
        public DateTime DOB { get; set; }
        public required Gender Gender { get; set; }
        public required string ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? AvatarImagePath { get; set; }
        public required AppUser User { get; set; }
    }
}