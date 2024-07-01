using Core.Auth.Services;
using Core.Infrastructure.Validator;
using Core.Services;
using FluentValidation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;

namespace Core.Models.Personal
{
    public class UpdateEmailRequest
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Origin { get; set; }
    }

    public class UpdateEmailRequestValidator : AbstractValidator<UpdateEmailRequest>
    {
        private readonly ICurrentUserService _currentUserService;
        public UpdateEmailRequestValidator(IUserService userService, IStringLocalizer<UpdateUserRequestValidator> T, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage(T["Invalid Email Address."])
                .Must((user, email, _) => !userService.ExistsWithEmailAsync(email, _currentUserService.GetCurrentUserId()).Result)
                    .WithMessage((_, email) => string.Format(T["Email {0} is already registered."], email));

            RuleFor(x => x.Password)
                .NotEmpty()
                .Must((user, password, _) => userService.VerifyCurrentPassword(_currentUserService.GetCurrentUserId(), password).Result)
                    .WithMessage(T["Invalid password."]);
        }
    }
}