using Core.Auth.Services;
using Services.Appoinmets;
namespace Core.Infrastructure.Hangfire
{
    public class PeriodicAppointmentSending
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public PeriodicAppointmentSending(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task SendPeriodicAppointments()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var appointmentService = scope.ServiceProvider.GetRequiredService<IAppoinmentService>();
                await appointmentService.PeriodicAppointment();
            }
        }
    }
}
