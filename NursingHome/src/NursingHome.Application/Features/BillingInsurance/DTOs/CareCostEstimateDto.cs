using System.Collections.Generic;

namespace NursingHome.Application.Features.BillingInsurance.DTOs;

public record CareCostEstimateDto(//Dữ liệu để đẩy lên frontend
    string ResidentName,
    string? RoomNumber,
    string? RoomType,
    long CarePlanId,
    string CarePlanStatus,
    string? LocTier,
    decimal DailyEstimate,
    decimal MonthlyEstimate,
    int MedicareDaysUsed,
    int MedicareTotalDays,
    string MedicareAlert,
    IReadOnlyList<CostBreakdownDto> Breakdowns
);  