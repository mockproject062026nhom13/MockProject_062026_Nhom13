using FluentValidation;
using NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Validators;

public class GetPreAdmissionScreeningDetailQueryValidator
    : AbstractValidator<GetPreAdmissionScreeningDetailQuery>
{
    public GetPreAdmissionScreeningDetailQueryValidator()
    {
        RuleFor(query => query.ScreeningId)
            .GreaterThan(0)
            .WithMessage("Screening ID must be greater than 0.");
    }
}
