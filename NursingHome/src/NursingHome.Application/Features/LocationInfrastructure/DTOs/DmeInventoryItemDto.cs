namespace NursingHome.Application.Features.LocationInfrastructure.DTOs;

public class DmeInventoryItemDto
{
    public long EquipmentId { get; init; }

    public string ItemName { get; init; } = null!;

    public long CategoryId { get; init; }

    public string CategoryName { get; init; } = null!;

    public string AssetTag { get; init; } = null!;

    public string Status { get; init; } = null!;

    public long? AssignedUserId { get; init; }

    public string? AssignedUserName { get; init; }

    public long? AssignedResidentId { get; init; }

    public string? AssignedResidentName { get; init; }

    public decimal UnitValue { get; init; }
}
