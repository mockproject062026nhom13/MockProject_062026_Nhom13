namespace NursingHome.Application.Features.LocationInfrastructure.DTOs;

public class ConsumableSupplyItemDto
{
    public long SupplyId { get; init; }

    public string ItemName { get; init; } = null!;

    public long CategoryId { get; init; }

    public string CategoryName { get; init; } = null!;

    public int StockOnHand { get; init; }

    public int Total { get; init; }

    public int ReorderThreshold { get; init; }

    public decimal UnitCost { get; init; }

    public decimal PrivatePayRate { get; init; }

    public string Status { get; init; } = null!;
}
