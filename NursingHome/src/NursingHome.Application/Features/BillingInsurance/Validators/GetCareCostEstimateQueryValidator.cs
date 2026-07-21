using FluentValidation;

namespace NursingHome.Application.Features.BillingInsurance.Queries;

public class GetCareCostEstimateQueryValidator : AbstractValidator<GetCareCostEstimateQuery>
{
    public GetCareCostEstimateQueryValidator()
    {
        RuleFor(x => x.CarePlanId)
            .GreaterThan(0).WithMessage("CarePlan ID is invalid.");
    }
}