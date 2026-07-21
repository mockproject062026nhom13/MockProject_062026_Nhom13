using FluentValidation;
using NursingHome.Application.Features.UserSecurity.Commands;

namespace NursingHome.Application.Features.UserSecurity.Validators;

public class ChangeUserStatusCommandValidator : AbstractValidator<ChangeUserStatusCommand>
{
    private static readonly string[] AllowedActions = { "DEACTIVATE", "REACTIVATE", "SUSPEND" };

    public ChangeUserStatusCommandValidator()
    {
        RuleFor(x => x.Action)
            .NotEmpty().WithMessage("Action không được để trống.")
            .Must(a => AllowedActions.Contains(a.Trim().ToUpperInvariant()))
            .WithMessage("Action phải là một trong: DEACTIVATE, REACTIVATE, SUSPEND.");
    }
}
