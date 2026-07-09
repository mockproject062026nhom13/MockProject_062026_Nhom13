namespace NursingHome.Application.Features.Auth.DTOs;

public record UserAuthDto(
    Guid Id,               
    string Email, 
    bool MfaEnabled,       
    string EmployeeCode    
);