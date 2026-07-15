using NursingHome.Domain.Enums;

namespace NursingHome.Domain.Entities;

public class User
{
    public long Id { get; private set; }

    public string EmployeeCode { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public string FirstName { get; private set; } = null!;

    public string? MiddleName { get; private set; }

    public string LastName { get; private set; } = null!;

    public string? LicenseNumber { get; private set; }

    public string? PhoneNumber { get; private set; }

    public UserStatus Status { get; private set; }

    public bool MfaEnabled { get; private set; }

    public DateTimeOffset? LastLoginAt { get; private set; }

    public long RoleId { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    private User()
    {
    }

    /// <summary>
    /// Constructor dùng khi tạo User mới.
    /// </summary>
    public User(
        string employeeCode,
        string email,
        string firstName,
        string lastName,
        long roleId)
    {
        EmployeeCode = employeeCode;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;

        Status = UserStatus.INACTIVE;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Chỉ UserFactory được phép gọi.
    /// </summary>
    internal User(
        long id,
        string employeeCode,
        string email,
        string passwordHash,
        string firstName,
        string? middleName,
        string lastName,
        string? licenseNumber,
        string? phoneNumber,
        UserStatus status,
        bool mfaEnabled,
        DateTimeOffset? lastLoginAt,
        long roleId,
        bool isDeleted,
        DateTimeOffset? deletedAt,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        Id = id;
        EmployeeCode = employeeCode;
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        LicenseNumber = licenseNumber;
        PhoneNumber = phoneNumber;
        Status = status;
        MfaEnabled = mfaEnabled;
        LastLoginAt = lastLoginAt;
        RoleId = roleId;
        IsDeleted = isDeleted;
        DeletedAt = deletedAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void ActivateAccount(
        string passwordHash,
        string? phoneNumber)
    {
        if (IsDeleted)
            throw new InvalidOperationException("User has been deleted.");

        if (Status == UserStatus.LOCKED)
            throw new InvalidOperationException("Account is LOCKED.");

        if (Status == UserStatus.ACTIVE)
            throw new InvalidOperationException("Account has already been activated.");

        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        Status = UserStatus.ACTIVE;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Lock()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Deleted user cannot be locked.");

        Status = UserStatus.LOCKED;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}