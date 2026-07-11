using FluentValidation;
using NursingHome.Application.Features.LocationInfrastructure.Queries;

namespace NursingHome.Application.Features.LocationInfrastructure.Validators;

public class GetEquipmentSupplyInventoryQueryValidator
    : AbstractValidator<GetEquipmentSupplyInventoryQuery>
{
    private static readonly string[] DmeStatuses =
    [
        "AVAILABLE",
        "IN_SERVICE",
        "UNDER_MAINTENANCE",
        "RETIRED"
    ];

    private static readonly string[] SupplyStatuses =
    [
        "OK",
        "LOW_STOCK",
        "OUT_OF_STOCK"
    ];

    public GetEquipmentSupplyInventoryQueryValidator()
    {
        RuleFor(query => query.DmePage)
            .GreaterThanOrEqualTo(1)
            .WithMessage("DME page must be greater than or equal to 1.");

        RuleFor(query => query.DmePageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("DME page size must be between 1 and 100.");

        RuleFor(query => query.SupplyPage)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Supply page must be greater than or equal to 1.");

        RuleFor(query => query.SupplyPageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Supply page size must be between 1 and 100.");

        RuleFor(query => query.FacilityId)
            .GreaterThan(0)
            .When(query => query.FacilityId.HasValue)
            .WithMessage("Facility ID must be greater than 0.");

        RuleFor(query => query.CategoryId)
            .GreaterThan(0)
            .When(query => query.CategoryId.HasValue)
            .WithMessage("Category ID must be greater than 0.");

        RuleFor(query => query.DmeStatus)
            .Must(status => DmeStatuses.Contains(status!.Trim()))
            .When(query => !string.IsNullOrWhiteSpace(query.DmeStatus))
            .WithMessage("DME status is invalid.");

        RuleFor(query => query.SupplyStatus)
            .Must(status => SupplyStatuses.Contains(status!.Trim()))
            .When(query => !string.IsNullOrWhiteSpace(query.SupplyStatus))
            .WithMessage("Supply status is invalid.");

        RuleFor(query => query.Search)
            .Must(search => search!.Trim().Length <= 200)
            .When(query => !string.IsNullOrWhiteSpace(query.Search))
            .WithMessage("Search cannot exceed 200 characters.");
    }
}
