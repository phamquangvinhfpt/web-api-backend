using BusinessObject.Data;
using BusinessObject.Enums;
using Core.Auth.Services;
using Core.Helpers;
using Core.Infrastructure.Notifications;
using Core.Models;
using Core.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUser;
        private readonly AppDbContext _context;
        private readonly IUriService _uriService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationService notificationService, ICurrentUserService currentUser, AppDbContext context, IUriService uriService, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _currentUser = currentUser;
            _context = context;
            _uriService = uriService;
            _logger = logger;
        }

        [HttpGet("test-send-notification")]
        public async Task<string> TestNotification(CancellationToken cancellationToken)
        {
            string userId = _currentUser.GetCurrentUserId();
            var notification = new BasicNotification
            {
                Message = "Hello, World!",
                Label = BasicNotification.LabelType.Information,
                Title = "Test Notification",
                Url = "/test"
            };

            await _notificationService.SendNotificationToUser(userId, notification, null, cancellationToken);

            return "Notification sent";
        }

        [HttpPost("send-to-all")]
        // [MustHavePermission(Action.Create, Resource.Notifications)]
        public async Task<string> SendNotificationToAllUsers(SendNotificationRequestToAllUsersRequest request, CancellationToken cancellationToken)
        {
            _ = _notificationService.SendNotificationToAllUsers(request.Notification, request.SendTime, cancellationToken);
            return "Notification sent";
        }

        [HttpPost("get-notifications")]
        public async Task<PagedResponse<List<NotificationDto>>> GetNotifications(GetListNotificationsRequest request)
        {
            request.UserId = Guid.Parse(_currentUser.GetCurrentUserId());
            var route = Request.Path.Value;
            var notifications = _context.Notifications
            .Where(x => x.UserId == request.UserId);

            if (request.IsRead.HasValue)
            {
                notifications = notifications.Where(x => x.IsRead == request.IsRead.Value);
            }

            var totalRecords = await notifications.CountAsync(); 

            var validFilter = new PaginationFilter(request.PageNumber, request.PageSize); 

            var pagedNotifications = await notifications
                .OrderByDescending(x => x.CreatedAt)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Message = x.Message,
                    Label = x.Label,
                    Url = x.Url,
                    IsRead = x.IsRead,
                    CreatedOn = x.CreatedAt,
                    UserId = x.UserId
                })
                .ToListAsync();
            var response = PaginationHelper.CreatePagedResponse(pagedNotifications, validFilter, totalRecords, _uriService, route);
            return response;
        }

        [HttpGet("count-unread")]
        public Task<int> CountUnreadNotifications()
        {
            var userId = Guid.Parse(_currentUser.GetCurrentUserId());
            return _context.Notifications.CountAsync(x => x.UserId == userId && !x.IsRead);
        }

        [HttpPut("update-read-status/{id}")]
        public async Task<string> UpdateReadStatus(Guid id)
        {
            try
            {
                var userId = Guid.Parse(_currentUser.GetCurrentUserId());
                var notification = _context.Notifications.FirstOrDefault(x => x.Id == id && x.UserId == userId);
                if (notification == null)
                {
                    return "Notification not found";
                }
                notification.UpdateIsRead();
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
                return "Notification updated";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notification read status");
                throw new Exception("Error updating notification read status");
            }
        }

        [HttpPut("read-all")]
        public async Task<string> ReadAllNotifications()
        {
            try
            {
                var userId = Guid.Parse(_currentUser.GetCurrentUserId());
                var notifications = _context.Notifications.Where(x => x.UserId == userId && !x.IsRead);
                foreach (var notification in notifications)
                {
                    notification.UpdateIsRead();
                    _context.Notifications.Update(notification);
                }
                await _context.SaveChangesAsync();
                return "All notifications read status updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating all notifications read status");
                throw new Exception("Error updating all notifications read status");
            }
        }
    }
}