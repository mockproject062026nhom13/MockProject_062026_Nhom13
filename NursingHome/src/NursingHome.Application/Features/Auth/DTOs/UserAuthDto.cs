namespace NursingHome.Application.Features.Auth.DTOs;

public record UserAuthDto(
    long Id,               
    string Email, 
    bool MfaEnabled,       
    string EmployeeCode    
);