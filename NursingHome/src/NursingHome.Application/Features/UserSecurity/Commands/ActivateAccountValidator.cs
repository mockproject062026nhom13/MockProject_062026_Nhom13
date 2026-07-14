using FluentValidation;

namespace NursingHome.Application.Features.UserSecurity.Commands;

public class ActivateAccountValidator : AbstractValidator<ActivateAccountCommand>
{
    public ActivateAccountValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is not in a valid format.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required.")
            .Equal(x => x.Password)
            .WithMessage("Confirm password does not match password.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{8,}$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number must contain at least 8 digits and may start with '+'.");
    }
}