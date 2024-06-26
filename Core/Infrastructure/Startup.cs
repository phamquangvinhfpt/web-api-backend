using System.Text;
using BusinessObject.Data;
using BusinessObject.Models;
using Core.Auth.Repository;
using Core.Auth.Services;
using Services.Clinics;
using Core.Enums;
using Core.Infrastructure.Exceptions;
using Core.Infrastructure.Hangfire;
using Core.Infrastructure.Middleware;
using Core.Infrastructure.Serilog;
using Core.Properties;
using Core.Repository;
using Core.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Serilog;
using Services.Dentist;
using Repository.FollowUpAppointments;
using Services.FollowUpAppointments;
using Services.Appoinmets;
using Repository.Appointments;
using Core.Auth.Permissions;
using Core.Infrastructure.reCAPTCHAv3;
using Core.Infrastructure.FileStorage;
using Microsoft.Extensions.FileProviders;
using Core.Infrastructure.Validator;
using Core.Infrastructure.SpeedSMS;

namespace Core.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddReCaptchav3(config);
            services.AddValidators();
            services.AddSpeedSMS(config);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    SaveSigninToken = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddAuthorization(options =>
            {
                foreach (var permission in Permissions.All)
                {
                    options.AddPolicy(permission.Name, policy =>
                        policy.Requirements.Add(new PermissionRequirement(permission.Name)));
                }
            });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Core"));
            });

            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(config.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

            services.AddHangfireServer();

            services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                // Not require username
                options.User.AllowedUserNameCharacters = null;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = ".NET-Core-DrDentist-API-8.0",
                    Version = "v1",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
                });
            });
            // Cors port http request
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("https://drdentist.me", "http://localhost:5173", "https://localhost:7124")
                        .AllowCredentials());
            });

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.AddScoped<IDentistRepository, DentistRepo>();
            services.AddScoped<IDentistService, DentistService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IReCAPTCHAv3Service, ReCAPTCHAv3Service>();
            services.AddScoped<IDentistService, DentistService>();
            services.AddScoped<IClinicsService, ClinicsService>();
            services.AddTransient<IMailService, MailService>();
            services.AddSingleton<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IFollowUpAppointmentService, RemindFollowAppointmentService>();
            services.AddScoped<IFollowUpAppointmentRepository, FollowUpAppointmentRepository>();
            services.AddScoped<IAppoinmentService, AppoinmentService>();
            services.AddScoped<IAppoinmentService, PeriodicAppointmentService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<TokenCleanupJob>();
            services.AddScoped<RemindFollowUpAppointment>();
            services.AddTransient<IDummyService, DummyService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            services.AddProblemDetails();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            RecurringJob.AddOrUpdate<TokenCleanupJob>("CleanupTokens", job => job.CleanupTokens(), Cron.Daily);
            RecurringJob.AddOrUpdate<RemindFollowUpAppointment>("RemindFolowAppointment", job => job.RemindFollowUpAppointments(), Cron.Daily);
            RecurringJob.AddOrUpdate<PeriodicAppointmentSending>("PeriodicSendingMail", job => job.SendPeriodicAppointments(), Cron.Minutely);
            app.UseMiddleware<TokenRevokedMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseCors("CorsPolicy");
            app.UseExceptionHandler();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                RequestPath = new PathString("/Files")
            });
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }

        public static void Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
                InitializeRoles(services).Wait();
            }
        }

        private static async Task InitializeRoles(IServiceProvider services)
        {
            var roleNames = Enum.GetNames(typeof(Roles));

            try
            {
                await SeedRole.Initialize(services, roleNames);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred seeding the DB.");
            }
        }
    }
}