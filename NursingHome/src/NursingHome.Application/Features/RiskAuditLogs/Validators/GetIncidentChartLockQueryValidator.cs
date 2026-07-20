using FluentValidation;
using NursingHome.Application.Features.RiskAuditLogs.Queries;

namespace NursingHome.Application.Features.RiskAuditLogs.Validators;

public sealed class GetIncidentChartLockQueryValidator
    : AbstractValidator<GetIncidentChartLockQuery>
{
    public GetIncidentChartLockQueryValidator()
    {
        RuleFor(query => query.IncidentId)
            .GreaterThan(0)
            .WithMessage("Incident ID must be greater than 0.");
    }
}
