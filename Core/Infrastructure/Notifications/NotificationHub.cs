using Core.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SendGrid.Helpers.Errors.Model;

namespace Core.Infrastructure.Notifications
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private readonly ICurrentUserService _currentUserService;

        public NotificationHub(ICurrentUserService currentUserService, ILogger<NotificationHub> logger)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public override async Task OnConnectedAsync()
        {
            if (_currentUserService.GetCurrentUserId() is null)
            {
                throw new UnauthorizedException("Authentication Failed.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"GroupUser-{_currentUserService.GetCurrentUserId()}");

            await base.OnConnectedAsync();

            _logger.LogInformation("A client connected to NotificationHub: {connectionId}", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"GroupUser-{_currentUserService.GetCurrentUserId()!}");

            await base.OnDisconnectedAsync(exception);

            _logger.LogInformation("A client disconnected from NotificationHub: {connectionId}", Context.ConnectionId);
        }
    }
}