using System.Security.Claims;
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
            await EnsureRole(roleManager, superAdminRole.Name, Permissions.SuperAdmin.ReviewClinicInfo);
            await EnsureRole(roleManager, superAdminRole.Name, Permissions.SuperAdmin.ReviewDentistInfo);
            await EnsureRole(roleManager, superAdminRole.Name, Permissions.SuperAdmin.ManageAccounts);

            // Add permissions claims to ClinicOwner role
            var clinicOwnerRole = new IdentityRole<Guid>("ClinicOwner");
            await EnsureRole(roleManager, clinicOwnerRole.Name, Permissions.ClinicOwners.RegisterClinicInfo);
            await EnsureRole(roleManager, clinicOwnerRole.Name, Permissions.ClinicOwners.RegisterDentistInfo);
            await EnsureRole(roleManager, clinicOwnerRole.Name, Permissions.ClinicOwners.ManageClinicSchedule);
            await EnsureRole(roleManager, clinicOwnerRole.Name, Permissions.ClinicOwners.ManagePatientInfo);
            await EnsureRole(roleManager, clinicOwnerRole.Name, Permissions.ClinicOwners.ManageDentistInfo);
            await EnsureRole(roleManager, clinicOwnerRole.Name, Permissions.ClinicOwners.ManageConversations);

            // Add permissions claims to Dentist role
            var dentistRole = new IdentityRole<Guid>("Dentist");
            await EnsureRole(roleManager, dentistRole.Name, Permissions.Dentists.ViewWeeklySchedule);
            await EnsureRole(roleManager, dentistRole.Name, Permissions.Dentists.ProposePeriodicSchedule);
            await EnsureRole(roleManager, dentistRole.Name, Permissions.Dentists.SendExamResult);
            await EnsureRole(roleManager, dentistRole.Name, Permissions.Dentists.ViewPatientHistory);
            await EnsureRole(roleManager, dentistRole.Name, Permissions.Dentists.ChatWithCustomer);

            // Add permissions claims to Customer role
            var customerRole = new IdentityRole<Guid>("Customer");
            await EnsureRole(roleManager, customerRole.Name, Permissions.Customers.BookAppointment);
            await EnsureRole(roleManager, customerRole.Name, Permissions.Customers.ReceiveNotification);
            await EnsureRole(roleManager, customerRole.Name, Permissions.Customers.BookPeriodicAppointment);
            await EnsureRole(roleManager, customerRole.Name, Permissions.Customers.ReceiveExamResult);
            await EnsureRole(roleManager, customerRole.Name, Permissions.Customers.ChatWithDentist);

            // Add permissions claims to Guest role
            var guestRole = new IdentityRole<Guid>("Guest");
            await EnsureRole(roleManager, guestRole.Name, Permissions.Guests.ViewClinicInfo);
            await EnsureRole(roleManager, guestRole.Name, Permissions.Guests.ViewSchedule);
            await EnsureRole(roleManager, guestRole.Name, Permissions.Guests.ViewServices);
            await EnsureRole(roleManager, guestRole.Name, Permissions.Guests.RegisterAccount);
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