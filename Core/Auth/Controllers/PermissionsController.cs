using System.Security.Claims;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Core.Auth.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "SuperAdmin")]
    public class PermissionsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public PermissionsController(RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("AddPermission/{Role}")]
        [Authorize(Permissions.SuperAdmin.ManageAccounts)]
        public async Task<IActionResult> AddPermissionsToRoles(string Role)
        {
            if (Role != null)
            {
                switch (Role)
                {
                    case "SuperAdmin":
                        var superAdmin = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(superAdmin, new Claim(CustomClaimTypes.Permission, Permissions.SuperAdmin.ReviewClinicInfo));
                        await _roleManager.AddClaimAsync(superAdmin, new Claim(CustomClaimTypes.Permission, Permissions.SuperAdmin.ReviewDentistInfo));
                        await _roleManager.AddClaimAsync(superAdmin, new Claim(CustomClaimTypes.Permission, Permissions.SuperAdmin.ManageAccounts));
                        break;
                    case "ClinicOwner":
                        var ClinicOwner = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(ClinicOwner, new Claim(CustomClaimTypes.Permission, Permissions.ClinicOwners.RegisterClinicInfo));
                        await _roleManager.AddClaimAsync(ClinicOwner, new Claim(CustomClaimTypes.Permission, Permissions.ClinicOwners.RegisterDentistInfo));
                        await _roleManager.AddClaimAsync(ClinicOwner, new Claim(CustomClaimTypes.Permission, Permissions.ClinicOwners.ManageClinicSchedule));
                        await _roleManager.AddClaimAsync(ClinicOwner, new Claim(CustomClaimTypes.Permission, Permissions.ClinicOwners.ManagePatientInfo));
                        await _roleManager.AddClaimAsync(ClinicOwner, new Claim(CustomClaimTypes.Permission, Permissions.ClinicOwners.ManageDentistInfo));
                        await _roleManager.AddClaimAsync(ClinicOwner, new Claim(CustomClaimTypes.Permission, Permissions.ClinicOwners.ManageConversations));
                        break;
                    case "Dentist":
                        var Dentist = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(Dentist, new Claim(CustomClaimTypes.Permission, Permissions.Dentists.ViewWeeklySchedule));
                        await _roleManager.AddClaimAsync(Dentist, new Claim(CustomClaimTypes.Permission, Permissions.Dentists.ProposePeriodicSchedule));
                        await _roleManager.AddClaimAsync(Dentist, new Claim(CustomClaimTypes.Permission, Permissions.Dentists.SendExamResult));
                        await _roleManager.AddClaimAsync(Dentist, new Claim(CustomClaimTypes.Permission, Permissions.Dentists.ViewPatientHistory));
                        await _roleManager.AddClaimAsync(Dentist, new Claim(CustomClaimTypes.Permission, Permissions.Dentists.ChatWithCustomer));
                        break;
                    case "Customer":
                        var Customer = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(Customer, new Claim(CustomClaimTypes.Permission, Permissions.Customers.BookAppointment));
                        await _roleManager.AddClaimAsync(Customer, new Claim(CustomClaimTypes.Permission, Permissions.Customers.ReceiveNotification));
                        await _roleManager.AddClaimAsync(Customer, new Claim(CustomClaimTypes.Permission, Permissions.Customers.BookPeriodicAppointment));
                        await _roleManager.AddClaimAsync(Customer, new Claim(CustomClaimTypes.Permission, Permissions.Customers.ReceiveExamResult));
                        await _roleManager.AddClaimAsync(Customer, new Claim(CustomClaimTypes.Permission, Permissions.Customers.ChatWithDentist));
                        break;
                    default:
                        var Guest = await _roleManager.FindByNameAsync("guest");
                        await _roleManager.AddClaimAsync(Guest, new Claim(CustomClaimTypes.Permission, Permissions.Guests.ViewClinicInfo));
                        await _roleManager.AddClaimAsync(Guest, new Claim(CustomClaimTypes.Permission, Permissions.Guests.ViewSchedule));
                        await _roleManager.AddClaimAsync(Guest, new Claim(CustomClaimTypes.Permission, Permissions.Guests.ViewServices));
                        await _roleManager.AddClaimAsync(Guest, new Claim(CustomClaimTypes.Permission, Permissions.Guests.RegisterAccount));
                        break;
                }

                var userRole = await _userManager.FindByNameAsync(Role);
                return Ok(userRole);
            }
            return BadRequest("Role not defined!");
        }
    }
}