using System.Text;
using BusinessObject.Data;
using BusinessObject.Models;
using Core.Auth;
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

namespace Core.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
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

            services.AddAuthorizationBuilder()
                .AddPolicy(Permissions.Guests.ViewClinicInfo, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Guests.ViewClinicInfo));
                })
                .AddPolicy(Permissions.Guests.ViewSchedule, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Guests.ViewSchedule));
                })
                .AddPolicy(Permissions.Guests.ViewServices, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Guests.ViewServices));
                })
                .AddPolicy(Permissions.Guests.RegisterAccount, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Guests.RegisterAccount));
                })
                .AddPolicy(Permissions.Customers.BookAppointment, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Customers.BookAppointment));
                })
                .AddPolicy(Permissions.Customers.ReceiveNotification, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Customers.ReceiveNotification));
                })
                .AddPolicy(Permissions.Customers.BookPeriodicAppointment, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Customers.BookPeriodicAppointment));
                })
                .AddPolicy(Permissions.Customers.ReceiveExamResult, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Customers.ReceiveExamResult));
                })
                .AddPolicy(Permissions.Customers.ChatWithDentist, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Customers.ChatWithDentist));
                })
                .AddPolicy(Permissions.Dentists.ViewWeeklySchedule, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Dentists.ViewWeeklySchedule));
                })
                .AddPolicy(Permissions.Dentists.ProposePeriodicSchedule, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Dentists.ProposePeriodicSchedule));
                })
                .AddPolicy(Permissions.Dentists.SendExamResult, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Dentists.SendExamResult));
                })
                .AddPolicy(Permissions.Dentists.ViewPatientHistory, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Dentists.ViewPatientHistory));
                })
                .AddPolicy(Permissions.Dentists.ChatWithCustomer, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Dentists.ChatWithCustomer));
                })
                .AddPolicy(Permissions.ClinicOwners.RegisterClinicInfo, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.ClinicOwners.RegisterClinicInfo));
                })
                .AddPolicy(Permissions.ClinicOwners.RegisterDentistInfo, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.ClinicOwners.RegisterDentistInfo));
                })
                .AddPolicy(Permissions.ClinicOwners.ManageClinicSchedule, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.ClinicOwners.ManageClinicSchedule));
                })
                .AddPolicy(Permissions.ClinicOwners.ManagePatientInfo, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.ClinicOwners.ManagePatientInfo));
                })
                .AddPolicy(Permissions.ClinicOwners.ManageDentistInfo, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.ClinicOwners.ManageDentistInfo));
                })
                .AddPolicy(Permissions.ClinicOwners.ManageConversations, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.ClinicOwners.ManageConversations));
                })
                .AddPolicy(Permissions.SuperAdmin.ReviewClinicInfo, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.SuperAdmin.ReviewClinicInfo));
                })
                .AddPolicy(Permissions.SuperAdmin.ReviewDentistInfo, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.SuperAdmin.ReviewDentistInfo));
                })
                .AddPolicy(Permissions.SuperAdmin.ManageAccounts, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.SuperAdmin.ManageAccounts));
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
                        .WithOrigins("http://localhost:5151", "https://localhost:7124", "https://drdentist.me", "http://localhost:5000")
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
            services.AddScoped<IDentistService, DentistService>();
            services.AddScoped<IClinicsService, ClinicsService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<TokenCleanupJob>();
            services.AddTransient<IDummyService, DummyService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new [] { new HangfireAuthorizationFilter() }
            });
            RecurringJob.AddOrUpdate<TokenCleanupJob>("CleanupTokens", job => job.CleanupTokens(), Cron.Daily);
            app.UseMiddleware<TokenRevokedMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseCors("CorsPolicy");
            app.UseExceptionHandler();
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

                SeedDataAsync(services, configuration);
            }
        }

        private static async Task InitializeRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var roleNames = Enum.GetNames(typeof(Roles));

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new IdentityRole<Guid>(roleName);
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role {roleName}");
                    }
                }
            }

            try
            {
                await SeedRole.Initialize(services, roleNames);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred seeding the DB.");
            }
        }

        private static void SeedDataAsync(IServiceProvider services, IConfiguration configuration)
        {
            try
            {
                SeedData.Initialize(services, configuration).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred seeding the DB.");
            }
        }
    }
}