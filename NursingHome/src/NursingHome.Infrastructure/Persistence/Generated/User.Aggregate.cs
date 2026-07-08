using System;
using System.Linq;

namespace NursingHome.Infrastructure.Persistence.Generated; 

public partial class user
{
    protected user()
    {
        
    }

    public user(string employeeCode, string email, string firstName, string? middleName, string lastName, string? phoneNumber, long roleId, string? licenseNumber = null)
    {
        this.id = Guid.NewGuid(); 
        this.employee_code = employeeCode;
        this.role_id = roleId;

        this.email = email;
        this.phone_number = phoneNumber;
        
        this.first_name = firstName;
        this.middle_name = middleName; 
        this.last_name = lastName;

        this.phone_number = phoneNumber;
        this.role_id = roleId;
        this.license_number = licenseNumber;

        this.status = "INVITED"; 
        
        //hardpassword tạm thời
        this.password_hash = "$2b$12$DummyHashForPendingActivationUsersDoNotUseThisDirectly";
        
        this.mfa_enabled = false;
        this.is_deleted = false;
        
        this.created_at = DateTimeOffset.UtcNow;
        this.updated_at = DateTimeOffset.UtcNow;
    }
}