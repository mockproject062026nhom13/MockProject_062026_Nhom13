namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class ResidentSummaryDto
{
    public long ResidentId { get; init; }

    public string FullName { get; init; } = null!;

    public DateOnly DateOfBirth { get; init; }

    public string? Gender { get; init; }

    public string Status { get; init; } = null!;
}
