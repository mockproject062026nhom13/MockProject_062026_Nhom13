using FluentValidation;
using NursingHome.Application.Features.RiskAuditLogs.Commands;

namespace NursingHome.Application.Features.RiskAuditLogs.Validators;

public sealed class UnlockResidentChartCommandValidator
    : AbstractValidator<UnlockResidentChartCommand>
{
    public UnlockResidentChartCommandValidator()
    {
        RuleFor(command => command.IncidentId)
            .GreaterThan(0)
            .WithMessage("Incident ID must be greater than 0.");

        RuleFor(command => command.Reason)
            .Must(reason => !string.IsNullOrWhiteSpace(reason))
            .WithMessage("Unlock reason is required.");
    }
}
