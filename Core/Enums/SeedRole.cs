using Microsoft.AspNetCore.Identity;

namespace Core.Enums
{
    public static class SeedRole
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string[] roleNames)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }
    }
}