namespace NursingHome.Api.Features.CareTasks;

public record CareTaskDto(
    string TaskType,
    string Status,
    DateTimeOffset ScheduledTime,
    string CarePlanStatus,
    string ResidentName,
    string RoomNumber,
    string BedNumber
);
