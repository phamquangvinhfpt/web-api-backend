using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Message : BaseEntity
    {
        public Guid SenderID { get; set; }
        public Guid ReceiverID { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public required AppUser Sender { get; set; }
        public required AppUser Receiver { get; set; }
    }
}