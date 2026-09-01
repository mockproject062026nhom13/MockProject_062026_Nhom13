namespace NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

public class CarePlanAcknowledgmentSummaryDto
{
    public long CarePlanId { get; init; }

    public string Status { get; init; } = string.Empty;

    public bool SignificantChangeFlag { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset UpdatedAt { get; init; }
}
