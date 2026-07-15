using FluentValidation;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Commands.UpdateStaffingRules;

public class UpdateStaffingRulesCommandValidator : AbstractValidator<UpdateStaffingRulesCommand>
{
    public UpdateStaffingRulesCommandValidator()
    {
        RuleFor(x => x.FacilityId)
            .GreaterThan(0).WithMessage("FacilityId must be greater than 0.");

        RuleForEach(x => x.Standard.Values).SetValidator(new ShiftRatiosValidator());
        RuleForEach(x => x.Emergency.Values).SetValidator(new ShiftRatiosValidator());
    }
}

public class ShiftRatiosValidator : AbstractValidator<ShiftRatiosDto>
{
    public ShiftRatiosValidator()
    {
        RuleFor(x => x.Day).SetValidator(new RatioDetailValidator());
        RuleFor(x => x.Evening).SetValidator(new RatioDetailValidator());
        RuleFor(x => x.Night).SetValidator(new RatioDetailValidator());
    }
}

public class RatioDetailValidator : AbstractValidator<RatioDetailDto>
{
    public RatioDetailValidator()
    {
        RuleFor(x => x.Nurse)
            .GreaterThan(0).WithMessage("Nurse staffing ratio must be greater than zero.");

        RuleFor(x => x.Cna)
            .GreaterThan(0).WithMessage("CNA staffing ratio must be greater than zero.");
    }
}
