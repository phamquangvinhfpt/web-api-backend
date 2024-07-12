using BusinessObject.Data;
using BusinessObject.Enums;
using BusinessObject.Models;
using Core.Infrastructure.Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationSender _notificationSender;
        private readonly AppDbContext _db;
        private readonly IJobService _jobService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
                UserManager<AppUser> userManager,
                INotificationSender notificationSender,
                AppDbContext db,
                IJobService jobService,
                ILogger<NotificationService> logger)
        {
            _userManager = userManager;
            _notificationSender = notificationSender;
            _db = db;
            _jobService = jobService;
            _logger = logger;
        }

        public async Task SendNotificationToAllUsers(BasicNotification notification, DateTime? sendTime, CancellationToken cancellationToken)
        {
            if (sendTime != null && sendTime > DateTime.Now)
            {
                TimeSpan timeSpan = sendTime.Value - DateTime.Now;
                _jobService.Schedule(() => ExcuteSendNotificationToAllUsers(notification, cancellationToken), timeSpan);
            }
            else
            {
                _jobService.Enqueue(() => ExcuteSendNotificationToAllUsers(notification, cancellationToken));
                Console.WriteLine("Notification sent to all users.");
            }
        }

        public async Task ExcuteSendNotificationToAllUsers(BasicNotification notification, CancellationToken cancellationToken)
        {
            List<string> userIds = await _userManager.Users.Select(u => u.Id.ToString()).ToListAsync(cancellationToken);
            await ExcuteSendNotificationToUsers(userIds, notification, cancellationToken);
            Console.WriteLine("Job executed.");
        }

        public async Task SendNotificationToUser(string userId, BasicNotification notification, DateTime? sendTime, CancellationToken cancellationToken)
        {
            if (sendTime != null && sendTime > DateTime.Now)
            {
                TimeSpan timeSpan = sendTime.Value - DateTime.Now;
                _jobService.Schedule(() => ExcuteSendNotificationToUser(userId, notification, cancellationToken), timeSpan);
            }
            else
            {
                _jobService.Enqueue(() => ExcuteSendNotificationToUser(userId, notification, cancellationToken));
            }
        }

        public async Task ExcuteSendNotificationToUser(string userId, BasicNotification notification, CancellationToken cancellationToken)
        {
            Notification addNoti = new Notification(
                 Guid.Parse(userId),
                 notification.Title,
                 notification.Label,
                 notification.Message,
                 notification.Url);

            _db.Notifications.Add(addNoti);
            await _db.SaveChangesAsync(cancellationToken);
            await _notificationSender.SendToUserAsync(notification, userId, cancellationToken);
            _logger.LogInformation($"Notification sent to user {userId}: {notification.Title}");
            Console.WriteLine($"Notification sent to user {userId}: {notification.Title}");
        }

        public async Task SendNotificationToUsers(List<string> userIds, BasicNotification notification, DateTime? sendTime, CancellationToken cancellationToken)
        {
            if (sendTime != null && sendTime > DateTime.Now)
            {
                TimeSpan timeSpan = sendTime.Value - DateTime.Now;
                _jobService.Schedule(() => ExcuteSendNotificationToUsers(userIds, notification, cancellationToken), timeSpan);
            }
            else
            {
                _jobService.Enqueue(() => ExcuteSendNotificationToUsers(userIds, notification, cancellationToken));
            }
        }

        public async Task ExcuteSendNotificationToUsers(List<string> userIds, BasicNotification notification, CancellationToken cancellationToken)
        {
            List<Notification> addNotis = new List<Notification>();
            foreach (string userId in userIds)
            {
                addNotis.Add(new Notification(
                                Guid.Parse(userId),
                                notification.Title,
                                notification.Label,
                                notification.Message,
                                notification.Url));
            }

            _db.Notifications.AddRange(addNotis);
            await _db.SaveChangesAsync(cancellationToken);
            await _notificationSender.SendToUsersAsync(notification, userIds, cancellationToken);
        }
    }
}