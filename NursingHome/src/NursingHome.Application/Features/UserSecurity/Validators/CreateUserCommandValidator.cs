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
            .Must(phone =>
            {
                var digits = new string(phone.Where(char.IsDigit).ToArray());

                return digits.Length == 10 ||
                       (digits.Length == 11 && digits.StartsWith("1"));
            })
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number must be a valid US phone number.");

        // Role
        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage("Please select a role.");
    }
}