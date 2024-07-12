using Serilog;
using ILogger = Serilog.ILogger;

namespace Core.Infrastructure.Notifications
{
    public static class Startup
    {
        public static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration config)
        {
            ILogger logger = Log.ForContext(typeof(Startup));
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
                hubOptions.MaximumReceiveMessageSize = 102400000;
            });
            logger.Information("SignalR is added to services.");
            return services;
        }

        internal static IEndpointRouteBuilder MapNotifications(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<NotificationHub>("/notifications", options =>
            {
                options.CloseOnAuthenticationExpiration = true;
            });
            Console.WriteLine("Notifications are mapped.");
            return endpoints;
        }
    }
}