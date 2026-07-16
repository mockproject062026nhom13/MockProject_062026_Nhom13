using System.Linq;
using FluentValidation;
using NursingHome.Application.Features.UserSecurity.Commands;

namespace NursingHome.Application.Features.UserSecurity.Validators;


public class ActivateAccountValidator : AbstractValidator<ActivateAccountCommand>
{
    public ActivateAccountValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email format is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one number.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required.")
            .Equal(x => x.Password)
            .WithMessage("Confirm password must match password.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required to activate your account.")
            .Matches(@"^\+[1-9]\d{7,14}$")
            .WithMessage(
                "Phone number must be in E.164 format (example: +84901234567)."
            );
    }
}