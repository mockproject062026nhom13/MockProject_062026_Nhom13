using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.Rbac.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories.Rbac;

public class RbacRepository : IRbacRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public RbacRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RolePermissionMatrixDto> GetRolePermissionMatrixAsync(CancellationToken cancellationToken)
    {
        var roles = await _dbContext.Roles
            .AsNoTracking()
            .Where(role => !role.IsDeleted)
            .OrderBy(role => role.RoleName)
            .Select(role => new RoleDirectoryItemDto
            {
                RoleId = role.Id,
                RoleName = role.RoleName,
                Description = role.Description
            })
            .ToListAsync(cancellationToken);

        var permissions = await _dbContext.Permissions
            .AsNoTracking()
            .OrderBy(permission => permission.ActionCode)
            .Select(permission => new PermissionItemDto
            {
                PermissionId = permission.Id,
                ActionCode = permission.ActionCode,
                IsPhiSensitive = permission.IsPhiSensitive
            })
            .ToListAsync(cancellationToken);

        var rolePermissions = await _dbContext.Roles
            .AsNoTracking()
            .Where(role => !role.IsDeleted)
            .SelectMany(
                role => role.Permissions,
                (role, permission) => new RolePermissionMappingDto
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id,
                    IsGranted = true
                })
            .OrderBy(mapping => mapping.RoleId)
            .ThenBy(mapping => mapping.PermissionId)
            .ToListAsync(cancellationToken);

        return new RolePermissionMatrixDto
        {
            Roles = roles,
            Permissions = permissions,
            RolePermissions = rolePermissions
        };
    }
}
