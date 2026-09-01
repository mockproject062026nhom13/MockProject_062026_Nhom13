namespace NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

public class CarePlanTaskSummaryDto
{
    public long TaskId { get; init; }

    public string TaskType { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTimeOffset ScheduledTime { get; init; }
}
