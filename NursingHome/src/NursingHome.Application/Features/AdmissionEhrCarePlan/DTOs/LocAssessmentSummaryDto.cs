namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class LocAssessmentSummaryDto
{
    public long AssessmentId { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public AssessedByDto AssessedBy { get; init; } = null!;
}
