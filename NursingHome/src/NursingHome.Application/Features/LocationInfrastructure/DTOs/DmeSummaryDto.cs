namespace NursingHome.Application.Features.LocationInfrastructure.DTOs;

public class DmeSummaryDto
{
    public int TotalDmeItems { get; init; }

    public int InUseCount { get; init; }

    public int UnderMaintenanceCount { get; init; }

    public decimal TotalAssetValue { get; init; }
}
