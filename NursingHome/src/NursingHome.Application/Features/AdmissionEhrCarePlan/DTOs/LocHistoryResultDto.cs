namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class LocHistoryResultDto
{
    public LocHistoryResidentDto Resident { get; init; } = null!;

    public IReadOnlyList<LocHistoryItemDto> HistoryItems { get; init; } = [];
}
