using MediatR;
using NursingHome.Application.Abstractions.UserSecurity;
using NursingHome.Application.Common;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.UserSecurity.Commands;

public class ChangeUserStatusCommandHandler(IUserAdminRepository repository)
    : IRequestHandler<ChangeUserStatusCommand, ApiResponse<bool>>
{
    private readonly IUserAdminRepository _repository = repository;

    public async Task<ApiResponse<bool>> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        var changed = await _repository.ChangeStatusAsync(request.Id, request.Action.Trim().ToUpperInvariant(), cancellationToken);

        if (!changed)
            throw new NotFoundException($"Không tìm thấy người dùng có ID là {request.Id}.");

        return ApiResponse<bool>.CreateSuccess(true, 200, "Cập nhật trạng thái người dùng thành công.");
    }
}
