namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class CareLevelSummaryDto
{
    public long CareLevelId { get; init; }

    public string LevelCode { get; init; } = null!;

    public string LevelName { get; init; } = null!;
}
