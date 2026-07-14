using FluentValidation;
using NursingHome.Application.Features.CareLevelResidents.Queries; 

namespace NursingHome.Application.Features.CareLevelResidents.Validators;

public class GetResidentListQueryValidator : AbstractValidator<GetResidentListQuery>
{
    public GetResidentListQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
            
        RuleFor(x => x.Status)
            .Must(status => new[] { "PENDING", "ACTIVE", "DISCHARGED", "DECEASED" }.Contains(status?.ToUpper()))
            .When(x => !string.IsNullOrWhiteSpace(x.Status))
            .WithMessage("Status must be one of: PENDING, ACTIVE, DISCHARGED, DECEASED.");

        RuleFor(x => x.PayerSource)
            .Must(payer => new[] { "MEDICARE", "MEDICAID", "PRIVATE_INSURANCE", "FAMILY" }.Contains(payer?.ToUpper()))
            .When(x => !string.IsNullOrWhiteSpace(x.PayerSource))
            .WithMessage("Payer source must be one of: MEDICARE, MEDICAID, PRIVATE_INSURANCE, FAMILY.");
    }
}