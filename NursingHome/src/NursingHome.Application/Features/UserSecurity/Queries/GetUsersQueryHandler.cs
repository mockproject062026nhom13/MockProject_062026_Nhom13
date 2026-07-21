using MediatR;
using NursingHome.Application.Abstractions.UserSecurity;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;

namespace NursingHome.Application.Features.UserSecurity.Queries;

public class GetUsersQueryHandler(IUserAdminRepository repository)
    : IRequestHandler<GetUsersQuery, ApiResponse<IReadOnlyList<UserListItemDto>>>
{
    private readonly IUserAdminRepository _repository = repository;

    public async Task<ApiResponse<IReadOnlyList<UserListItemDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 20 : request.PageSize;

        var (items, total) = await _repository.GetUsersAsync(
            request.Search, request.RoleId, request.Status, page, pageSize, cancellationToken);

        var totalPages = total > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 0;

        var pagination = new PaginationMetadata(
            Page: page,
            PageSize: pageSize,
            TotalPages: totalPages,
            TotalItems: total,
            HasNext: page < totalPages,
            HasPrevious: page > 1
        );

        return ApiResponse<IReadOnlyList<UserListItemDto>>.CreateSuccess(
            data: items,
            statusCode: 200,
            message: "Lấy danh sách người dùng thành công.",
            pagination: pagination
        );
    }
}
