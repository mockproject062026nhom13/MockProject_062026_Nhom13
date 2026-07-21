namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class AdlItemDto
{
    public long MetricId { get; init; }

    public string MetricName { get; init; } = null!;

    public string Category { get; init; } = null!;

    public int Score { get; init; }

    public string? Notes { get; init; }
}
