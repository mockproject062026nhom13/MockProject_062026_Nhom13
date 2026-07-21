namespace NursingHome.Application.Features.CarePlans.DTOs;

public record InterventionResultDto(
    long InterventionId,
    string AssignedRole,
    string? Description,
    string? TaskType,
    string? Frequency
);

public record CareAreaResultDto(
    long GoalId,
    string GoalStatus,
    string? Name,
    string? Source,
    string? Goal,
    string? Measure,
    DateOnly? TargetDate,
    List<InterventionResultDto> Interventions
);

public record CarePlanDetailDto(
    long Id,
    long ResidentId,
    string Status,
    string StatusFlow,   // "Draft → Pending Review → Active"
    bool SignificantChangeFlag,
    List<CareAreaResultDto> CareAreas
);

// Kết quả kích hoạt.
public record CarePlanActivationResultDto(
    long CarePlanId,
    string Status,
    int TasksGenerated
);

// Ảnh chụp nhẹ để handler kiểm tra trạng thái/điều kiện.
public record CarePlanSnapshot(
    long Id,
    string Status,
    bool IsDeleted,
    int GoalCount,
    int InterventionCount
);
