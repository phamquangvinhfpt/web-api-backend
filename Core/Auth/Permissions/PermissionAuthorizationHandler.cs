using BusinessObject.Models;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Core.Auth.Permissions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        UserManager<AppUser> _userManager;
        RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _config;

        public PermissionAuthorizationHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;
        }

        protected override async Task<ResponseManager> HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User != null)
            {
                // Get all the roles the user belongs to and check if any of the roles has the permission required

                // for the authorization to succeed.
                var user = await _userManager.GetUserAsync(context.User);
                if (user != null)
                {
                    var permissions = context.User.Claims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                            x.Value == requirement.Permission);

                    if (permissions.Any())
                    {
                        context.Succeed(requirement);
                        return new ResponseManager
                        {
                            IsSuccess = true,
                            Message = "User is authorized!"
                        };
                    }
                }
            };

            return new ResponseManager
            { 
                IsSuccess = false,
                Message = "User not authenticated!, Please Login to continue!",
            };
        }
    }
}