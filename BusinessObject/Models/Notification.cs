using static BusinessObject.Enums.BasicNotification;

namespace BusinessObject.Models
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string Title { get; set; }
        public LabelType Label { get; set; }
        public string Message { get; set; }
        public string? Url { get; set; }
        public bool IsRead { get; set; }

        public Notification(Guid userId, string title, LabelType label, string message, string? url)
        {
            UserId = userId;
            Title = title;
            Label = label;
            Message = message;
            IsRead = false;
            Url = url;
        }

        public void UpdateIsRead()
        {
            IsRead = !IsRead;
        }
    }

    public interface INotificationMessage
    {
    }
}