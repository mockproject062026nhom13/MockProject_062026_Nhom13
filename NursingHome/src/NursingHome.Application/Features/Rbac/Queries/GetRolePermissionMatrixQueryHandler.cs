using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Rbac.DTOs;

namespace NursingHome.Application.Features.Rbac.Queries;

public class GetRolePermissionMatrixQueryHandler
    : IRequestHandler<GetRolePermissionMatrixQuery, ApiResponse<RolePermissionMatrixDto>>
{
    private readonly IRbacRepository _rbacRepository;

    public GetRolePermissionMatrixQueryHandler(IRbacRepository rbacRepository)
    {
        _rbacRepository = rbacRepository;
    }

    public async Task<ApiResponse<RolePermissionMatrixDto>> Handle(
        GetRolePermissionMatrixQuery request,
        CancellationToken cancellationToken)
    {
        var matrix = await _rbacRepository.GetRolePermissionMatrixAsync(cancellationToken);

        return ApiResponse<RolePermissionMatrixDto>.CreateSuccess(matrix);
    }
}
