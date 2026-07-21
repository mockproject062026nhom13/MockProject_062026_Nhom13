using MediatR;
using NursingHome.Application.Abstractions.UserSecurity;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.UserSecurity.Queries;

public class GetUserByIdQueryHandler(IUserAdminRepository repository)
    : IRequestHandler<GetUserByIdQuery, ApiResponse<UserDetailDto>>
{
    private readonly IUserAdminRepository _repository = repository;

    public async Task<ApiResponse<UserDetailDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            throw new NotFoundException($"Không tìm thấy người dùng có ID là {request.Id}.");

        return ApiResponse<UserDetailDto>.CreateSuccess(
            data: user,
            statusCode: 200,
            message: "Lấy thông tin người dùng thành công."
        );
    }
}
