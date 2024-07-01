using Core.Infrastructure.Validator;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Core.Models.Personal
{
    public class ChangePasswordRequest
    {
        public string Password { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmNewPassword { get; set; } = default!;
    }

    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator(IStringLocalizer<ChangePasswordRequestValidator> T)
        {
            RuleFor(p => p.Password)
                .NotEmpty();

            RuleFor(p => p.NewPassword)
                .NotEmpty();

            RuleFor(p => p.ConfirmNewPassword)
                .Equal(p => p.NewPassword)
                    .WithMessage(T["Passwords do not match."]);
        }
    }
}