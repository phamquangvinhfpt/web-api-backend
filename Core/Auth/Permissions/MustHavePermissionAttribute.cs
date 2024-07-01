using Microsoft.AspNetCore.Authorization;

namespace Core.Auth.Permissions
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = Permission.NameFor(action, resource);
    }
}