namespace NursingHome.Application.Features.CareLevelResidents.DTOs;

public class CarePlanInfoDto
{
    public long ResidentId { get; set; }

    public string FullName { get; set; } = null!;

    public long? LevelOfCare { get; set; }

    public string? Bed { get; set; }

    public string? RoomNumber { get; set; }

    public string Status {get;set;} = string.Empty;

    public List<CareTaskAssignmentDto> CareTasks { get; set; } = [];
}


public class CareTaskAssignmentDto
{
    public string TaskType { get; set; } = null!;

    public long AssignedCnaId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string FullName { get; set; } = null!;
}
public sealed class UpdateCarePlanStatusRequest
{
    public string Status { get; set; } = null!;
}
public class UpdateCarePlanStatusResponse
{
    public long CarePlanId { get; set; }
    public string Status { get; set; } = default!;
    public DateTimeOffset UpdatedAt { get; set; }
}
/*
get id of careplan
query id trong care_interventions	where x.care_plan_id = idcareplan
query trong care_task where x.care_intervention_id = idcare_interventions laays ra assigned_cna_id + task_type
dungf assigned_cna_id query trong bảng user lấy ra employee_code + first_name+ middle_name+ last_name
*/