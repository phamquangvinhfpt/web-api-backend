using BusinessObject.Models;
using Core.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Core.Auth.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "SuperAdmin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // /api/Roles
        [HttpGet("roles")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(new UserResponseManager
            {
                IsSuccess = true,
                Message = roles,
            });
        }

        // /api/Roles/{RoleName}
        [HttpPost("roles")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                 var newRole = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName.Trim()));
            }
            return Ok(new UserResponseManager
            {
                IsSuccess = true,
                Message = "Role '" + roleName + "' has been added to Role Manager!",
            });
        }

        // /api/Roles/{RoleName}
        [HttpDelete("roles")]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            if (roleName != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var removeRole = await _roleManager.DeleteAsync(role);
            }
            return Ok(new UserResponseManager
            {
                IsSuccess = true,
                Message = "Role '" + roleName + "' has been Removed from Role Manager!",
            });
        }

        // /api/userRoles/{id}
        [HttpGet("roles/userId")]
        public async Task<IActionResult> GetUserRolebyId(Guid userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());
            if (existingUser != null)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                return Ok(roles);
            };
            return BadRequest("User not found!");

        }

        // /api/AddUserRole/{RoleName}
        [HttpPost("add-user-role/role-name")]
        public async Task<IActionResult> AddUserRole(Guid userId, string userRole)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());
            if (userRole != null)
            {
                if (await _roleManager.RoleExistsAsync(userRole))
                {
                    await _userManager.AddToRoleAsync(existingUser, userRole);
                    var addedRoles = await _userManager.GetRolesAsync(existingUser);
                    return Ok(addedRoles);
                };
                return BadRequest("Role does not exits!");
            }
            return NotFound("Please fill the required fields ! ");
        }

        // /api/RemoveUserRole/{RoleName}
        [HttpDelete("remove-user-role/user-role")]
        public async Task<IActionResult> RemoveUserRole(Guid userId, string userRole)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());
            if (userRole != null)
            {
                if (await _roleManager.RoleExistsAsync(userRole))
                {
                    await _userManager.RemoveFromRoleAsync(existingUser, userRole);
                    var addedRoles = await _userManager.GetRolesAsync(existingUser);
                    return Ok(addedRoles);
                };
                return BadRequest("Role does not exits!");
            }
            return NotFound("Please fill the required fields ! ");
        }
    }
}