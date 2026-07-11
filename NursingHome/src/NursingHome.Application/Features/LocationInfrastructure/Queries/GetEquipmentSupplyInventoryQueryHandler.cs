using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;

namespace NursingHome.Application.Features.LocationInfrastructure.Queries;

public class GetEquipmentSupplyInventoryQueryHandler
    : IRequestHandler<GetEquipmentSupplyInventoryQuery, ApiResponse<EquipmentSupplyInventoryDto>>
{
    private readonly IInventoryRepository _inventoryRepository;

    public GetEquipmentSupplyInventoryQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<ApiResponse<EquipmentSupplyInventoryDto>> Handle(
        GetEquipmentSupplyInventoryQuery request,
        CancellationToken cancellationToken)
    {
        var inventory = await _inventoryRepository.GetEquipmentSupplyInventoryAsync(
            request,
            cancellationToken);

        return ApiResponse<EquipmentSupplyInventoryDto>.CreateSuccess(inventory);
    }
}
