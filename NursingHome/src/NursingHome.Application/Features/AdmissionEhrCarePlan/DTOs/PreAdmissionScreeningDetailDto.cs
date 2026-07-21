namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class PreAdmissionScreeningDetailDto
{
    public long ScreeningId { get; init; }

    public string Status { get; init; } = null!;

    public DateTimeOffset CreatedAt { get; init; }

    public ResidentSummaryDto Resident { get; init; } = null!;

    public UserSummaryDto ScreenedBy { get; init; } = null!;
}
