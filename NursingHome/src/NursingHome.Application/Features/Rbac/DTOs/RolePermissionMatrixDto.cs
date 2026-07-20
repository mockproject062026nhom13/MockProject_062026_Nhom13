namespace NursingHome.Application.Features.Rbac.DTOs;

public class RolePermissionMatrixDto
{
    public IReadOnlyList<RoleDirectoryItemDto> Roles { get; init; } = [];

    public IReadOnlyList<PermissionItemDto> Permissions { get; init; } = [];

    public IReadOnlyList<RolePermissionMappingDto> RolePermissions { get; init; } = [];
}

public class RoleDirectoryItemDto
{
    public long RoleId { get; init; }

    public string RoleName { get; init; } = null!;

    public string? Description { get; init; }
}

public class PermissionItemDto
{
    public long PermissionId { get; init; }

    public string ActionCode { get; init; } = null!;

    public bool IsPhiSensitive { get; init; }
}

public class RolePermissionMappingDto
{
    public long RoleId { get; init; }

    public long PermissionId { get; init; }

    public bool IsGranted { get; init; }
}
