using System.ComponentModel.DataAnnotations;

namespace Core.Models.AuthModels
{
    public class AuthUser
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        required public string Password { get; set; }
    }
}