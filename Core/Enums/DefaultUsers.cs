using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Auth;
using Core.Auth.Permissions;
using Microsoft.AspNetCore.Identity;

namespace Core.Enums
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@root.com";
            var adminUserName = "admin";
            var adminPassword = "123Pa$$word!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.SuperAdmin.ToString());
                    foreach (var permission in Permissions.All)
                    {
                        await userManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, permission.Name));
                    }
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user. Errors: {errors}");
                }
            }
        }
    }
}