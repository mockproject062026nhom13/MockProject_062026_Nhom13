namespace NursingHome.Application.Features.CarePlans.DTOs;

// SC_027 — payload tạo/sửa care plan.
// Chỉ AssignedRole được lưu (care_interventions.assigned_role); các field mô tả được echo lại
// trong response nhưng KHÔNG persist vì schema care_goals/care_interventions không có cột text.

public record CarePlanInterventionInput(
    string AssignedRole,
    string? Description = null,
    string? TaskType = null,
    string? Frequency = null
);

public record CareAreaInput(
    string? Name = null,          // vd "Mobility" — echo
    string? Source = null,        // SUGGESTED / MANUAL — echo
    string? Goal = null,          // nội dung mục tiêu — echo
    string? Measure = null,       // vd "Braden score" — echo
    DateOnly? TargetDate = null,  // echo
    List<CarePlanInterventionInput>? Interventions = null
);
