using System.Globalization;
using Core.Models.Personal;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace Core.Infrastructure.Validator
{
    public static class Startup
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateEmailRequestValidator>());
            return services;
        }
    }
}