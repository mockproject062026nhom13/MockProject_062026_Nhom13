namespace NursingHome.Application.Abstractions.BillingInsurance;

public record CareCostDataDto(//Get data from database
    long CarePlanId,
    string CarePlanStatus,
    string ResidentFirstName,
    string? ResidentMiddleName,
    string ResidentLastName,
    string? RoomNumber,
    string? RoomType,
    string? CareLevelName,
    long? CareLevelRateId,
    decimal LocDailyRate
);