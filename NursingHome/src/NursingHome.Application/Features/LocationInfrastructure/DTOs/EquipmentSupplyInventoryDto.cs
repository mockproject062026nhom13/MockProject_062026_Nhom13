namespace NursingHome.Application.Features.LocationInfrastructure.DTOs;

public class EquipmentSupplyInventoryDto
{
    public DmeSummaryDto Summary { get; init; } = new();

    public PagedResultDto<DmeInventoryItemDto> DurableMedicalEquipment { get; init; } = new();

    public PagedResultDto<ConsumableSupplyItemDto> ConsumableSupplies { get; init; } = new();
}
