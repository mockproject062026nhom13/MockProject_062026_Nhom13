using FluentValidation;

namespace NursingHome.Application.Features.UserSecurity.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Identifier)
            .NotEmpty().WithMessage("Identifier (Email or Phone) is required.")
            .MaximumLength(255).WithMessage("Identifier must not exceed 255 characters.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
