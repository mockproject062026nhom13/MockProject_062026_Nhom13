using System.Threading;
using System.Threading.Tasks;
using NursingHome.Application.Features.Rbac.DTOs;

namespace NursingHome.Application.Abstractions;

public interface IRbacRepository
{
    Task<RolePermissionMatrixDto> GetRolePermissionMatrixAsync(CancellationToken cancellationToken);
}
