using BusinessObject.Enums;

namespace Core.Infrastructure.Notifications
{
    public interface INotificationService
    {
        Task SendNotificationToAllUsers(BasicNotification notification, DateTime? sendTime, CancellationToken cancellationToken);
        Task SendNotificationToUser(string userId, BasicNotification notification, DateTime? sendTime, CancellationToken cancellationToken);
        Task SendNotificationToUsers(List<string> userIds, BasicNotification notification, DateTime? sendTime, CancellationToken cancellationToken);
    }
}