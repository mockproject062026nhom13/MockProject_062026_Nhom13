using FluentValidation;
using NursingHome.Application.Features.CarePlanAcknowledgments.Queries;

namespace NursingHome.Application.Features.CarePlanAcknowledgments.Validators;

public class GetCarePlanAcknowledgmentQueryValidator
    : AbstractValidator<GetCarePlanAcknowledgmentQuery>
{
    public GetCarePlanAcknowledgmentQueryValidator()
    {
        RuleFor(query => query.CarePlanId)
            .GreaterThan(0)
            .WithMessage("Care plan ID must be greater than 0.");
    }
}
