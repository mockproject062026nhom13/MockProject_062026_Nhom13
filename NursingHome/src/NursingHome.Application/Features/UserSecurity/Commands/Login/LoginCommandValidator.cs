using FluentValidation;

namespace NursingHome.Application.Features.UserSecurity.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.EmployeeCode)
            .NotEmpty().WithMessage("EmployeeCode is required.")
            .MaximumLength(50).WithMessage("EmployeeCode must not exceed 50 characters.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
