using System.Reflection;
using System.Security.Claims;
using BusinessObject.Data;
using BusinessObject.Models;
using Core.Auth.Permissions;
using Core.Auth.Services;
using Core.Infrastructure.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Enums
{
    public static class SeedRole
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string[] roleNames)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
            await SeedRoleClaimsAsync(roleManager);
            await SeedBasicUserAsync(userManager, roleManager);
            await InitializeAsync(serviceProvider);
        }

        public static async Task SeedBasicUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            var rootPassword = "123Pa$$word!";
            var image_url = "Files/Image/jpg/ad.jpg";

            var admin = new AppUser
                {
                    FullName = "Admin",
                    UserName = "admin",
                    BirthDate = new DateOnly(2001, 1, 1),
                    PhoneNumber = "0942705605",
                    Email = "admin@root.com",
                    EmailConfirmed = true,
                    ImageUrl = image_url
                };
            
            var owner = new AppUser
                {
                    FullName = "Owner",
                    UserName = "owner",
                    BirthDate = new DateOnly(2001, 1, 1),
                    PhoneNumber = "0942705605",
                    Email = "owner@root.com",
                    EmailConfirmed = true,
                    ImageUrl = image_url
                };
            
            var dentist = new AppUser
                {
                    FullName = "Dentist",
                    UserName = "dentist",
                    BirthDate = new DateOnly(2001, 1, 1),
                    PhoneNumber = "0942705605",
                    Email = "dentist@root.com",
                    EmailConfirmed = true,
                    ImageUrl = image_url
                };

            var customer = new AppUser
                {
                    FullName = "Customer",
                    UserName = "customer",
                    BirthDate = new DateOnly(2001, 1, 1),
                    PhoneNumber = "0942705605",
                    Email = "customer@root.com",
                    EmailConfirmed = true,
                    ImageUrl = image_url
                };

            var result = await userManager.CreateAsync(admin, rootPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Roles.SuperAdmin.ToString());
            } else {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create admin user. Errors: {errors}");
            }

            result = await userManager.CreateAsync(owner, rootPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(owner, Roles.ClinicOwner.ToString());
            } else {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create owner user. Errors: {errors}");
            }

            result = await userManager.CreateAsync(dentist, rootPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(dentist, Roles.Dentist.ToString());
            } else {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create dentist user. Errors: {errors}");
            }

            result = await userManager.CreateAsync(customer, rootPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customer, Roles.Customer.ToString());
            } else {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create customer user. Errors: {errors}");
            }
        }

        public static async Task SeedRoleClaimsAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>())
            {
                var roleEntity = await roleManager.FindByNameAsync(role.ToString());
                if (roleEntity == null)
                {
                    continue; // Skip if role doesn't exist
                }

                // Get the permissions for the current role
                IReadOnlyList<Permission> rolePermissions;
                switch (role)
                {
                    case Roles.SuperAdmin:
                        rolePermissions = Permissions.SuperAdmin;
                        break;
                    case Roles.ClinicOwner:
                        rolePermissions = Permissions.ClinicOwner;
                        break;
                    case Roles.Dentist:
                        rolePermissions = Permissions.Dentist;
                        break;
                    case Roles.Customer:
                        rolePermissions = Permissions.Customer;
                        break;
                    case Roles.Guest:
                        rolePermissions = Permissions.Guest;
                        break;
                    default:
                        rolePermissions = new List<Permission>().AsReadOnly(); // Empty list for unknown roles
                        break;
                }

                // Remove existing claims for this role
                var existingClaims = await roleManager.GetClaimsAsync(roleEntity);
                foreach (var claim in existingClaims)
                {
                    await roleManager.RemoveClaimAsync(roleEntity, claim);
                }

                // Add new claims based on the role's permissions
                foreach (var permission in rolePermissions)
                {
                    await roleManager.AddClaimAsync(roleEntity, new Claim(CustomClaimTypes.Permission, permission.Name));
                }
            }
        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path!, "Infrastructure");
            string dataPath = Path.Combine(path!, "Notifications", "NotificationData.json");
            var _logger = serviceProvider.GetRequiredService<ILogger<NotificationSeeder>>();
            var _serializerService = serviceProvider.GetRequiredService<ISerializerService>();
            var _db = serviceProvider.GetRequiredService<AppDbContext>();
            if (!_db.Notifications.Any())
            {
                _logger.LogInformation("Started to Seed Notifications.");
                string notificationData = await File.ReadAllTextAsync(dataPath);
                var notifications = _serializerService.Deserialize<List<Notification>>(notificationData);
                var users = await _db.Users.Where(u => u.UserName == "admin").FirstOrDefaultAsync();
                foreach (var notification in notifications)
                {
                    notification.UserId = users.Id;
                    _db.Notifications.Add(notification);
                }
            }
            await _db.SaveChangesAsync();

            _logger.LogInformation("Seeded Notifications.");
        }
    }
}