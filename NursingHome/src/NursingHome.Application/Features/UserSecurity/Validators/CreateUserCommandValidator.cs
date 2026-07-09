using System.Linq;
using FluentValidation;
using NursingHome.Application.Features.UserSecurity.Commands;

namespace NursingHome.Application.Features.UserSecurity.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        // Check empty fullname
        RuleFor(x => x.FullName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

        // Check empty and format email
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        // US Phone Number
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\d{10}|1\d{10})$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number must contain exactly 10 digits or 11 digits starting with 1.");

        // Role
        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage("Please select a role.");
    }
}