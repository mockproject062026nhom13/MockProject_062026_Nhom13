using FluentValidation;
using NursingHome.Application.Features.RiskAuditLogs.Commands;

namespace NursingHome.Application.Features.RiskAuditLogs.Validators;

public class UpdateSlaConfigCommandValidator : AbstractValidator<UpdateSlaConfigCommand>
{
    public UpdateSlaConfigCommandValidator()
    {
        RuleFor(x => x.SlaWindowHrs)
            .InclusiveBetween(1, 168)
            .WithMessage("Thời hạn SLA (giờ) phải nằm trong khoảng 1 đến 168.");
    }
}
