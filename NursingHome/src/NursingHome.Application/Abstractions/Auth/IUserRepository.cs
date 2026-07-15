using NursingHome.Application.Features.Auth.DTOs;

namespace NursingHome.Application.Abstractions.Auth;

public interface IUserRepository
{
    Task<UserAuthDto?> GetUserAuthInfoByEmailAsync(string email);
    Task UpdateLoginTimeAsync(long userId);

    // Check unique email
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);

    // Check exists role
    Task<bool> IsRoleExistsAsync(long roleId, CancellationToken cancellationToken);

    // Check exists Facility
    Task<bool> IsFacilityExistsAsync(long facilityId, CancellationToken cancellationToken);

    Task<long> AddUserAsync(UserCreationDto dto, CancellationToken cancellationToken);

    // Get max employee_code
    Task<string?> GetLastEmployeeCodeAsync(CancellationToken cancellationToken);
}

public class UserCreationDto
{
    public string EmployeeCode { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public long RoleId { get; set; }
    public long? AssignedFacilityId { get; set; }
}
