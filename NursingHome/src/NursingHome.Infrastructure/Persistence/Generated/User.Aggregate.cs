using System;
using System.Linq;

namespace NursingHome.Infrastructure.Persistence.Generated; 

public partial class User
{
    protected User()
    {
        
    }

    public User(string employeeCode, string email, string firstName, string? middleName, string lastName, string? phoneNumber, long roleId, string? licenseNumber = null)
    {
        this.EmployeeCode = employeeCode;
        this.Email = email;
        this.FirstName = firstName;
        this.MiddleName = middleName; 
        this.LastName = lastName;
        this.PhoneNumber = phoneNumber;
        this.RoleId = roleId;
        this.LicenseNumber = licenseNumber;

        this.Status = "INVITED"; 
  
        this.PasswordHash = "$2b$12$DummyHashForPendingActivationUsersDoNotUseThisDirectly";
        
        this.MfaEnabled = false;
        this.IsDeleted = false;
        
        this.CreatedAt = DateTimeOffset.UtcNow;
        this.UpdatedAt = DateTimeOffset.UtcNow;
    }
}