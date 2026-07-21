using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

// SC_004 / AD-01 — hành vi quản trị tài khoản. Đặt trong file partial riêng để không
// đụng phần scaffold generated (giống mẫu user.ActivateAccount.cs).
public partial class User
{
    public void UpdateProfile(string firstName, string? middleName, string lastName,
        string? phoneNumber, string? licenseNumber, long roleId)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        LicenseNumber = licenseNumber;
        RoleId = roleId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    // Deactivate = soft-delete (AD-04), giữ lịch sử.
    public void Deactivate()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
        Status = "INACTIVE";
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Reactivate()
    {
        IsDeleted = false;
        DeletedAt = null;
        Status = "ACTIVE";
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Suspend()
    {
        Status = "LOCKED";
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
