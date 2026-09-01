namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public class UserSummaryDto
{
    public long UserId { get; init; }

    public string FullName { get; init; } = null!;

    public string Email { get; init; } = null!;

    public long RoleId { get; init; }

    public string RoleName { get; init; } = null!;
}
