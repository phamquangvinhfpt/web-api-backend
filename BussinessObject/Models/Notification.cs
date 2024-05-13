using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessObject.Enums;

namespace BussinessObject.Models
{
    public class Notification : BaseEntity
    {
        public Guid UserID { get; set; }
        public required string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsRead { get; set; }
        public required AppUser User { get; set; }
    }
}