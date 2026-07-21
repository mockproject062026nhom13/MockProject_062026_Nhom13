using MediatR;
using NursingHome.Application.Abstractions.UserSecurity;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;

namespace NursingHome.Application.Features.UserSecurity.Queries;

public class GetUserStatisticsQueryHandler(IUserAdminRepository repository)
    : IRequestHandler<GetUserStatisticsQuery, ApiResponse<UserStatisticsDto>>
{
    private readonly IUserAdminRepository _repository = repository;

    public async Task<ApiResponse<UserStatisticsDto>> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
    {
        var stats = await _repository.GetStatisticsAsync(cancellationToken);

        return ApiResponse<UserStatisticsDto>.CreateSuccess(
            data: stats,
            statusCode: 200,
            message: "Lấy thống kê người dùng thành công."
        );
    }
}
