using Core.Auth.Services;
using Core.Services;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Core.Models.Personal
{
    public class UpdatePhoneNumberRequest
    {
        public string? UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }

    public class UpdatePhoneNumberRequestValidator : AbstractValidator<UpdatePhoneNumberRequest>
    {
        private readonly ICurrentUserService _currentUserService;
        public UpdatePhoneNumberRequestValidator(IUserService userService, IStringLocalizer<UpdatePhoneNumberRequestValidator> T, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
                .Must((user, phone, _) => !userService.ExistsWithPhoneNumberAsync(phone!, _currentUserService.GetCurrentUserId()).Result)
                    .WithMessage((_, phone) => string.Format(T["Phone number {0} is already registered."], phone))
                    .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));

            RuleFor(x => x.Password)
                .NotEmpty()
                .Must((user, password, _) => userService.VerifyCurrentPassword(_currentUserService.GetCurrentUserId(), password).Result)
                    .WithMessage(T["Invalid password."]);
        }
    }
}