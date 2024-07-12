using BusinessObject.Models;
using Core.Auth.Services;
using Microsoft.AspNetCore.SignalR;
using static Core.Infrastructure.Notifications.NotificationConstants;

namespace Core.Infrastructure.Notifications
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly ICurrentUserService _currentUserService;

        public NotificationSender(IHubContext<NotificationHub> notificationHubContext, ICurrentUserService currentUserService) =>
            (_notificationHubContext, _currentUserService) = (notificationHubContext, currentUserService);

        public Task BroadcastAsync(INotificationMessage notification, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.All
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task BroadcastAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.AllExcept(excludedConnectionIds)
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Group($"GroupUser-{_currentUserService.GetCurrentUserId()!}")
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.GroupExcept($"GroupUser-{_currentUserService.GetCurrentUserId()!}", excludedConnectionIds)
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Group(group)
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task SendToGroupAsync(INotificationMessage notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.GroupExcept(group, excludedConnectionIds)
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Groups(groupNames)
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.User(userId)
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);

        public Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken) =>
            _notificationHubContext.Clients.Users(userIds)
                .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
    }
}