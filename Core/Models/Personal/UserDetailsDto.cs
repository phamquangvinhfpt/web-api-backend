using BusinessObject.Enums;

namespace Core.Models.Personal
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public Gender? Gender { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string? Email { get; set; }

        public UserStatus Status { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }
        public string? ImageUrl { get; set; }
    }
}