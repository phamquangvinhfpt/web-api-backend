using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.SpeedSMS
{
    public static class Startup
    {
        public static IServiceCollection AddSpeedSMS(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SpeedSMSSettings>(config.GetSection(nameof(SpeedSMSSettings)));
            services.AddScoped<ISpeedSMSService, SpeedSMSService>();
            return services;
        }
            
    }
}