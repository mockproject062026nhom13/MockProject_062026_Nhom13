using FluentValidation;
using NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Validators;

public class GetLocHistoryQueryValidator : AbstractValidator<GetLocHistoryQuery>
{
    public GetLocHistoryQueryValidator()
    {
        RuleFor(query => query.ResidentId)
            .GreaterThan(0)
            .WithMessage("Resident ID must be greater than 0.");
    }
}
