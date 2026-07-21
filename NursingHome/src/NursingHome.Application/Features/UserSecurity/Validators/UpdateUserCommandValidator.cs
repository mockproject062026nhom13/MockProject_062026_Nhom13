using FluentValidation;
using NursingHome.Application.Features.UserSecurity.Commands;

namespace NursingHome.Application.Features.UserSecurity.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên không được để trống.")
            .MaximumLength(300);

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("RoleId không hợp lệ.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
