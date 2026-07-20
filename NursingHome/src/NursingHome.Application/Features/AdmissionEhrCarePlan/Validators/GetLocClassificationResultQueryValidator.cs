using FluentValidation;
using NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Validators;

public class GetLocClassificationResultQueryValidator
    : AbstractValidator<GetLocClassificationResultQuery>
{
    public GetLocClassificationResultQueryValidator()
    {
        RuleFor(query => query.AssessmentId)
            .GreaterThan(0)
            .WithMessage("Assessment ID must be greater than 0.");
    }
}
