using System.Reflection;
using BusinessObject.Data;
using BusinessObject.Models;
using Core.Auth.Services;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Notifications
{
    public class NotificationSeeder : ICustomSeeder
    {
        private readonly ISerializerService _serializerService;
        private readonly AppDbContext _db;
        private readonly ILogger<NotificationSeeder> _logger;

        public NotificationSeeder(ISerializerService serializerService, AppDbContext db, ILogger<NotificationSeeder> logger)
        {
            _serializerService = serializerService;
            _db = db;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dataPath = Path.Combine(path!, "Notifications", "NotificationData.json");
            if (!_db.Notifications.Any())
            {
                _logger.LogInformation("Started to Seed Notifications.");
                string notificationData = await File.ReadAllTextAsync(dataPath);
                var notifications = _serializerService.Deserialize<List<Notification>>(notificationData);
                var users = await _db.Users.Where(u => u.UserName == "admin").FirstOrDefaultAsync();
                foreach (var notification in notifications)
                {
                    notification.UserId = users.Id;
                    _db.Notifications.Add(notification);
                }
            }
            await _db.SaveChangesAsync();

            _logger.LogInformation("Seeded Notifications.");
        }
    }

    public interface ICustomSeeder
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}