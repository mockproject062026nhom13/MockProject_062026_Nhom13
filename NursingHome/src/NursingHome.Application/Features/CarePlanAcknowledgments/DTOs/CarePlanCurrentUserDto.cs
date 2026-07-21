namespace NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

public class CarePlanCurrentUserDto
{
    public long UserId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public long RoleId { get; init; }

    public string RoleName { get; init; } = string.Empty;

    public string? LicenseNumber { get; init; }
}
