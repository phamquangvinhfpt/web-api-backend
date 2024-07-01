namespace Core.Models.AuthModels
{
    public class PermissionViewModel
    {
        public Guid RoleId { get; set; }
        public IList<RoleClaimsViewModel> RoleClaims { get; set; }
    }
    public class RoleClaimsViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}