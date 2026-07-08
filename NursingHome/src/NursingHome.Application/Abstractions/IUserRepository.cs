using System.Threading;
using System.Threading.Tasks;

namespace NursingHome.Application.Abstractions;

public class UserCreationDto
{
    public string EmployeeCode { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public long RoleId { get; set; }
    public Guid? AssignedFacilityId { get; set; }
}

public interface IUserRepository
{
    // Check unique email
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    
    Task<Guid> AddUserAsync(UserCreationDto dto, CancellationToken cancellationToken);

    // Get max employee_code
    Task<string?> GetLastEmployeeCodeAsync(CancellationToken cancellationToken);
}