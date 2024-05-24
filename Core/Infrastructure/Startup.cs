using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Data;
using BussinessObject.Models;
using Core.Auth;
using Core.Enums;
using Core.Infrastructure.Exceptions;
using Core.Infrastructure.Serilog;
using Core.Properties;
using Core.Repository;
using Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

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
                .AddPolicy(Permissions.Users.View, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Users.View));
                })
                .AddPolicy(Permissions.Users.Create, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Users.Create));
                })
                .AddPolicy(Permissions.Users.Edit, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Users.Edit));
                })
                .AddPolicy(Permissions.Users.Delete, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Users.Delete));
                })
                .AddPolicy(Permissions.Users.ViewById, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Users.ViewById));
                })
                .AddPolicy(Permissions.Users.SuperAdminView, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Users.SuperAdminView));
                })
                .AddPolicy(Permissions.Users.SuperAdminCreate, builder =>
                {
                    builder.AddRequirements(new PermissionRequirement(Permissions.Users.SuperAdminCreate));
                });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Core"));
            });

            services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
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
                    Title = ".NET-Core-UserManagement-API-8.0",
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
                        .WithOrigins("http://localhost:5151", "https://localhost:7124", "https://drdentist.me")
                        .AllowCredentials());
            });

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.Configure<MailSettings>(config.GetSection("MailSettings"));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IMailService, MailService>();

            services.AddTransient<IDummyService, DummyService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
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