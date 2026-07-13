using FluentValidation;

namespace NursingHome.Application.Features.Facilities.Commands.UpdateStaffingConfig;

public class UpdateStaffingConfigCommandValidator : AbstractValidator<UpdateStaffingConfigCommand>
{
    public UpdateStaffingConfigCommandValidator()
    {
        RuleFor(x => x.FacilityId)
            .GreaterThan(0).WithMessage("FacilityId must be greater than 0.");

        RuleFor(x => x.MinHrsPerResidentDay)
            .GreaterThan(0).WithMessage("MinHrsPerResidentDay must be greater than 0.")
            .LessThan(24).WithMessage("MinHrsPerResidentDay cannot exceed 24 hours.");

        RuleFor(x => x.WarnBelowPercentage)
            .GreaterThan(0).WithMessage("WarnBelowPercentage must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("WarnBelowPercentage cannot exceed 100.");
    }
}
