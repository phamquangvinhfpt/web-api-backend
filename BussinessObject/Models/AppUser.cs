using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessObject.Enums;
using Microsoft.AspNetCore.Identity;

namespace BussinessObject.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public UserStatus Status { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        public virtual List<Message> SentMessages { get; set; }
        public virtual List<Message> ReceivedMessages { get; set; }
        public virtual List<Notification> Notifications { get; set; }
    }
}