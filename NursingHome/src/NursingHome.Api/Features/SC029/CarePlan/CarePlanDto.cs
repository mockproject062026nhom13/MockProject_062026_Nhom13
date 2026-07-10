namespace NursingHome.Api.Features.CarePlan;

public record CarePlanDto(
    long CareLevelId,
    decimal DailyRate,
    string RoomNumber,
    string BedNumber,
    string CareName,
    string Description,
    DateTime UpdatedAt
);
