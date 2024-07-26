using System.Security.Claims;
using BusinessObject.Models;
using Core.Auth.Permissions;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Action = Core.Auth.Permissions.Action;
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

        [HttpPost("add-permission")]
        public async Task<IActionResult> AddPermissionsToUser(string UserId, string Action, string Resource)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest("User not defined!");
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound($"User '{UserId}' not found.");
            }
            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any(x => x.Type == CustomClaimTypes.Permission && x.Value == $"Permissions.{Resource}.{Action}"))
            {
                return BadRequest($"User '{user.FullName}' already has the permission '{Action}' on '{Resource}'.");
            }
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, $"Permissions.{Resource}.{Action}"));
            return Ok($"Permissions added to user '{user.FullName}' successfully.");
        }

        [HttpDelete("remove-permission")]
        public async Task<IActionResult> RemovePermissionsFromUser(string UserId, string Action, string Resource)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest("User not defined!");
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound($"User '{UserId}' not found.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var permission = userClaims.FirstOrDefault(x => x.Type == CustomClaimTypes.Permission && x.Value == $"Permissions.{Resource}.{Action}");

            if (permission == null)
            {
                return BadRequest($"User '{user.FullName}' does not have the permission '{Action}' on '{Resource}'.");
            }

            await _userManager.RemoveClaimAsync(user, permission);
            return Ok($"Permissions removed from user '{user.FullName}' successfully.");
        }

        [HttpGet("permissions")]
        [MustHavePermission(Action.View, Resource.Permission)]
        public IActionResult GetPermissions()
        {
            return Ok(Core.Auth.Permissions.Permissions.All);
        }

        [HttpGet("permissions/role")]
        public async Task<IActionResult> GetPermissions(string RoleId)
        {
            if (string.IsNullOrEmpty(RoleId))
            {
                return BadRequest("Role not defined!");
            }

            var roleEntity = await _roleManager.FindByIdAsync(RoleId);
            if (roleEntity == null)
            {
                return NotFound($"Role '{RoleId}' not found.");
            }

            var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
            var permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission)
                                        .Select(x => x.Value);
            return Ok(permissions);
        }

        [HttpGet("permissions/user")]
        public async Task<IActionResult> GetPermissionsByUser(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest("User not defined!");
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound($"User '{UserId}' not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();
            foreach (var role in userRoles)
            {
                var roleEntity = await _roleManager.FindByNameAsync(role);
                if (roleEntity == null)
                {
                    return NotFound($"Role '{role}' not found.");
                }

                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                var userClaims = await _userManager.GetClaimsAsync(user);
                // Combine role claims and user claims
                permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission)
                                        .Select(x => x.Value)
                                        .Union(userClaims.Where(x => x.Type == CustomClaimTypes.Permission)
                                        .Select(x => x.Value))
                                        .ToList();
            }
            return Ok(permissions);
        }
    }
}