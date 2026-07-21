using NursingHome.Application.Features.UserSecurity.DTOs;

namespace NursingHome.Application.Abstractions.UserSecurity;

// SC_004 / AD-01 — quản trị tài khoản (list/detail/edit/đổi trạng thái).
public interface IUserAdminRepository
{
    Task<(IReadOnlyList<UserListItemDto> Items, int TotalCount)> GetUsersAsync(
        string? search, long? roleId, string? status, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<UserStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default);

    Task<UserDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> UpdateProfileAsync(
        long id, string firstName, string? middleName, string lastName,
        string? phoneNumber, string? licenseNumber, long roleId, CancellationToken cancellationToken = default);

    Task<bool> ChangeStatusAsync(long id, string action, CancellationToken cancellationToken = default);
}
