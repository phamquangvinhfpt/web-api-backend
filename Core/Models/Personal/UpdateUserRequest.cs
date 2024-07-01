using BusinessObject.Enums;
using Core.Infrastructure.Validator;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Core.Models.Personal
{
    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public Gender Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
    }

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator(IStringLocalizer<UpdateUserRequestValidator> T)
        {
            RuleFor(p => p.FullName)
               .MaximumLength(75);

            RuleFor(p => p.BirthDate)
                .LessThan(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(T["Birth date must be in the past."]);
        }
    }
}