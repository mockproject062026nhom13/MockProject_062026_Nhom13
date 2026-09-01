namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class AssessedByDto
{
    public long UserId { get; init; }

    public string FullName { get; init; } = null!;

    public long RoleId { get; init; }

    public string RoleName { get; init; } = null!;
}
