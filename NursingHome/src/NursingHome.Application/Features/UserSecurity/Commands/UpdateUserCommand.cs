using MediatR;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.UserSecurity.Commands;

// Email không cho sửa (identity field §5G.2) — chỉ sửa tên, phone, role, license.
public record UpdateUserCommand(
    long Id,
    string FullName,
    string? PhoneNumber,
    long RoleId,
    string? LicenseNumber
) : IRequest<ApiResponse<bool>>;
