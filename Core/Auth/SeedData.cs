using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Auth
{
    public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            
            // Add permissions claims to SuperAdmin role
            var superAdminRole = new IdentityRole<Guid>("SuperAdmin");
            await EnsureRole(roleManager, superAdminRole.Name, Permissions.Users.SuperAdminView);
            await EnsureRole(roleManager, superAdminRole.Name, Permissions.Users.SuperAdminCreate);
            
            var adminRole = new IdentityRole<Guid>("Admin");
            await EnsureRole(roleManager, adminRole.Name, Permissions.Users.View);
            await EnsureRole(roleManager, adminRole.Name, Permissions.Users.Create);
            await EnsureRole(roleManager, adminRole.Name, Permissions.Users.Edit);
            await EnsureRole(roleManager, adminRole.Name, Permissions.Users.ViewById);

            var agentRole = new IdentityRole<Guid>("Agent");
            await EnsureRole(roleManager, agentRole.Name, Permissions.Users.View);
            await EnsureRole(roleManager, agentRole.Name, Permissions.Users.Edit);
            await EnsureRole(roleManager, agentRole.Name, Permissions.Users.ViewById);

            var clientRole = new IdentityRole<Guid>("Client");
            await EnsureRole(roleManager, clientRole.Name, Permissions.Users.ViewById);
            // Add other roles and permissions as required
        }
    }

    private static async Task EnsureRole(RoleManager<IdentityRole<Guid>> roleManager, string roleName, string permission)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new IdentityRole<Guid>(roleName);
            await roleManager.CreateAsync(role);
        }

        var claims = await roleManager.GetClaimsAsync(role);
        if (!claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
        }
    }
}
}