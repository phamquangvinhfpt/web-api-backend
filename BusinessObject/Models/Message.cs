namespace BusinessObject.Models
{
    public class Message : BaseEntity
    {
        public Guid SenderID { get; set; }
        public Guid ReceiverID { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}