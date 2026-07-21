namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class LocClassificationResultDto
{
    public LocAssessmentSummaryDto Assessment { get; init; } = null!;

    public LocResidentSummaryDto Resident { get; init; } = null!;

    public AdlSummaryDto AdlSummary { get; init; } = null!;

    public IReadOnlyList<AdlItemDto> AdlItems { get; init; } = [];

    public CareLevelSummaryDto SuggestedLoc { get; init; } = null!;

    public CareLevelSummaryDto ConfirmedLoc { get; init; } = null!;

    public bool IsOverridden { get; init; }
}
