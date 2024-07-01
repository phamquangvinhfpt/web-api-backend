using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.reCAPTCHAv3
{
    public static class Startup
    {
        public static IServiceCollection AddReCaptchav3(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<reCAPTCHAv3Settings>(config.GetSection(nameof(reCAPTCHAv3Settings)));
            services.AddHttpClient<IReCAPTCHAv3Service, ReCAPTCHAv3Service>();
            return services;
        }
    }
}