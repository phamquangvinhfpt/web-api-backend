using BusinessObject.Data;
using BusinessObject.Models;
using Core.Auth.Services;
using Core.Enums;
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
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager, AppDbContext context, ICurrentUserService currentUser)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _currentUser = currentUser;
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

        // /api/UpdateUserRole/{RoleType}
        [HttpPut("update-user-role/{roleType}")]
        public async Task<IActionResult> UpdateUserRole(Guid userId, Roles roleType)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(userId.ToString());
                if (roleType != null)
                {
                    var roles = await _userManager.GetRolesAsync(existingUser);
                    await _userManager.RemoveFromRolesAsync(existingUser, roles.ToArray());
                    await _userManager.AddToRoleAsync(existingUser, roleType.ToString());
                    var addedRoles = await _userManager.GetRolesAsync(existingUser);
                    return Ok(addedRoles);
                }
                return NotFound("Please fill the required fields ! ");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // /api/LockUserAccount/{id}
        [HttpPut("lock-user-account")]
        public async Task<IActionResult> LockUserAccount(Guid userId)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(userId.ToString());
                if (existingUser != null)
                {
                    existingUser.LockoutEnabled = true;
                    existingUser.LockoutEnd = DateTime.Now.AddYears(100);
                    await _userManager.UpdateAsync(existingUser);
                    var userTokens = await _context.Tokens.Where(t => t.UserId == userId).ToListAsync();
                    _context.Tokens.RemoveRange(userTokens);
                    var currentUser = _currentUser.GetCurrentUserId();
                    await _context.SaveChangesAsync(Guid.Parse(currentUser));
                    return Ok("User Account has been locked!");
                }
                return BadRequest("User not found!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // /api/UnlockUserAccount/{id}
        [HttpPut("unlock-user-account")]
        public async Task<IActionResult> UnlockUserAccount(Guid userId)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(userId.ToString());
                if (existingUser != null)
                {
                    existingUser.LockoutEnabled = false;
                    existingUser.LockoutEnd = null;
                    await _userManager.UpdateAsync(existingUser);
                    return Ok("User Account has been unlocked!");
                }
                return BadRequest("User not found!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}