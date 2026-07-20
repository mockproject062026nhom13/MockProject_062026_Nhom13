using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;

namespace NursingHome.Application.Features.LocationInfrastructure.Queries;

public class GetEquipmentSupplyInventoryQuery : IRequest<ApiResponse<EquipmentSupplyInventoryDto>>
{
    public long? FacilityId { get; set; }

    public long? CategoryId { get; set; }

    public string? DmeStatus { get; set; }

    public string? SupplyStatus { get; set; }

    public string? Search { get; set; }

    public int DmePage { get; set; } = 1;

    public int DmePageSize { get; set; } = 20;

    public int SupplyPage { get; set; } = 1;

    public int SupplyPageSize { get; set; } = 20;
}
