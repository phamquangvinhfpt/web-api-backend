using Core.Auth.Services;
using Services.FollowUpAppointments;

namespace Core.Infrastructure.Hangfire
{
    public class RemindFollowUpAppointment
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RemindFollowUpAppointment(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task RemindFollowUpAppointments()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFollowUpAppointmentService>();

                await service.RemindFollowUpAppointment();
            }
        }
    }
}
