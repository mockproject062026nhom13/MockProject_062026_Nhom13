namespace NursingHome.Application.Features.UserSecurity.DTOs;

public class ActivateAccountRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
}

public class ActivateAccountResponse
{
    public string Email { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}