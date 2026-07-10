using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Rbac.DTOs;

namespace NursingHome.Application.Features.Rbac.Queries;

public class GetRolePermissionMatrixQuery : IRequest<ApiResponse<RolePermissionMatrixDto>>
{
}
