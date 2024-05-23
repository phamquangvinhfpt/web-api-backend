using System.Text;
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
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("starting server.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Host.UseSerilog((context, logger) =>
    {
        logger.MinimumLevel.Warning();
        logger.WriteTo.Console();
        logger.ReadFrom.Configuration(context.Configuration);
    });
    builder.Services.AddAutoMapper(typeof(Program));
    // Add Authentication
    //Authentication
    builder.Services.AddAuthentication(x =>
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],       // Jwt:Issuer - config value 
            ValidAudience = builder.Configuration["Jwt:Issuer"],     // Jwt:Issuer - config value 
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), // Jwt:Key - config value

            ClockSkew = TimeSpan.Zero
        };
    });

    //Authorization
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Permissions.Users.View, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(Permissions.Users.View));
        });

        options.AddPolicy(Permissions.Users.Create, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(Permissions.Users.Create));
        });

        options.AddPolicy(Permissions.Users.Edit, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(Permissions.Users.Edit));
        });

        options.AddPolicy(Permissions.Users.Delete, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(Permissions.Users.Delete));
        });
        options.AddPolicy(Permissions.Users.ViewById, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(Permissions.Users.ViewById));
        });
        options.AddPolicy(Permissions.Users.SuperAdminView, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(Permissions.Users.SuperAdminView));
        });
        options.AddPolicy(Permissions.Users.SuperAdminCreate, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(Permissions.Users.SuperAdminCreate));
        });
    });

    // Config Database
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("Core"));
    });
    builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
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
    // .AddApiEndpoints();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
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

    builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

    // Registering Interface
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddTransient<IMailService, MailService>();

    builder.Services.AddTransient<IDummyService, DummyService>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        // If the database is not created, create it
        context.Database.EnsureCreated();
        // context.Database.Migrate();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var roleNames = Enum.GetNames(typeof(Roles));
        foreach (var roleName in roleNames)
        {
            var roleExist = roleManager.RoleExistsAsync(roleName).Result;
            if (!roleExist)
            {
                var role = new IdentityRole<Guid>(roleName);
                var result = roleManager.CreateAsync(role).Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create role " + roleName);
                }
            }
        }

        try { await SeedRole.Initialize(scope.ServiceProvider, roleNames); }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred seeding the DB.");
        }

        try { await SeedData.Initialize(scope.ServiceProvider, app.Configuration); }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred seeding the DB.");
        }
    }

    // app.MapIdentityApi<AppUser>();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            c.RoutePrefix = string.Empty;
        });
    }
    app.UseHttpsRedirection();
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseExceptionHandler();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}