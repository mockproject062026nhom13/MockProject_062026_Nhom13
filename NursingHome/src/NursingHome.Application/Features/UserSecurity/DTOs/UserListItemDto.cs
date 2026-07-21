namespace NursingHome.Application.Features.UserSecurity.DTOs;

// SC_004 / AD-01 — một dòng trong User List.
public record UserListItemDto(
    long Id,
    string Name,
    string Email,
    string? PhoneNumber,
    long RoleId,
    string RoleName,
    string Status,          // giá trị thô trong DB: INVITED/ACTIVE/INACTIVE/LOCKED
    string StatusDisplay,   // hiển thị: Invited/Active/Suspended/Deactivated
    bool MfaEnabled,        // 2FA
    DateTimeOffset? LastLoginAt
);
