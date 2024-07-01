using System.Security.Claims;
using BusinessObject.Models;
using Core.Auth;
using Core.Auth.Permissions;
using Microsoft.AspNetCore.Identity;

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
        }

        public static async Task SeedBasicUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            var adminEmail = "admin@root.com";
            var adminUserName = "admin";
            var adminPassword = "123Pa$$word!";
            var image_url = "Files/Image/jpg/ad.jpg";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new AppUser
                {
                    FullName = "Admin",
                    UserName = adminUserName,
                    BirthDate = new DateOnly(2001, 1, 1),
                    PhoneNumber = "0942705605",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    ImageUrl = image_url
                };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.SuperAdmin.ToString());
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user. Errors: {errors}");
                }
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
    }
}