namespace NursingHome.Application.Features.UserSecurity.DTOs;

// SC_004 — chi tiết user để đổ vào form Edit.
public record UserDetailDto(
    long Id,
    string EmployeeCode,
    string FirstName,
    string? MiddleName,
    string LastName,
    string FullName,
    string Email,
    string? PhoneNumber,
    long RoleId,
    string RoleName,
    string? LicenseNumber,
    string Status,
    string StatusDisplay,
    bool MfaEnabled,
    DateTimeOffset? LastLoginAt,
    bool IsDeleted,
    DateTimeOffset CreatedAt
);
