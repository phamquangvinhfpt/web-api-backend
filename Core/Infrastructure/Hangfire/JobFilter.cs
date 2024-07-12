using Core.Auth.Services;
using Hangfire.Client;
using Hangfire.Logging;

namespace Core.Infrastructure.Hangfire
{
    public class JobFilter : IClientFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IServiceProvider _services;

        public JobFilter(IServiceProvider services) => _services = services;

        public void OnCreating(CreatingContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            Logger.InfoFormat("Set UserId parameters to job {0}.{1}...", context.Job.Method.ReflectedType?.FullName, context.Job.Method.Name);

            using var scope = _services.CreateScope();

            var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext;

            if (httpContext != null)
            {
                var userInfo = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();
                string? userId = userInfo.GetCurrentUserId();
                context.SetJobParameter("UserId", userId);
            }
            else
            {
                Logger.Info("Creating a system job or job without HttpContext");
                context.SetJobParameter("UserId", "System");
            }
        }

        public void OnCreated(CreatedContext context) =>
            Logger.InfoFormat(
                "Job created with parameters {0}",
                context.Parameters.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + ";" + s2));
    }
}