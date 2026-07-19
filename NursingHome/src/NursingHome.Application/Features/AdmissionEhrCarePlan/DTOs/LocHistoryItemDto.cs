namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class LocHistoryItemDto
{
    public long HistoryId { get; init; }

    public long CareLevelId { get; init; }

    public string LevelCode { get; init; } = null!;

    public string LevelName { get; init; } = null!;

    public DateOnly StartDate { get; init; }

    public DateOnly? EndDate { get; init; }

    public bool IsCurrent { get; init; }
}
