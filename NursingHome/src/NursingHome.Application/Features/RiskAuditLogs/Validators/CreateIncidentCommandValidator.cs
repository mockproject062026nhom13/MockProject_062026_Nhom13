using FluentValidation;
using NursingHome.Application.Features.RiskAuditLogs.Commands;

namespace NursingHome.Application.Features.RiskAuditLogs.Validators;

public class CreateIncidentCommandValidator : AbstractValidator<CreateIncidentCommand>
{
    public CreateIncidentCommandValidator()
    {
        RuleFor(x => x.ResidentId).GreaterThan(0).WithMessage("Please select a resident.");
        
        RuleFor(x => x.IncidentType)
            .Must(type => new[] { "FALL", "MEDICATION_ERROR", "ALTERCATION", "SKIN_TEAR" }.Contains(type))
            .WithMessage("Invalid incident type.");

        RuleFor(x => x.SeverityId).InclusiveBetween(1, 4).WithMessage("Please select a valid severity level.");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.ImmediateActionsTaken).NotEmpty().WithMessage("Immediate actions taken is required.");
        RuleFor(x => x.Witness).MaximumLength(255).WithMessage("Witness name cannot exceed 255 characters.");
        RuleFor(x => x.ReportedAt).NotEmpty().WithMessage("Date and time of incident is required.");
    }
}