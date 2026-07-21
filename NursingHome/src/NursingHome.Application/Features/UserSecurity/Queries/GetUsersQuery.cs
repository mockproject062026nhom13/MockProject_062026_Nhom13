using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;

namespace NursingHome.Application.Features.UserSecurity.Queries;

public record GetUsersQuery(
    string? Search,
    long? RoleId,
    string? Status,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<IReadOnlyList<UserListItemDto>>>;
