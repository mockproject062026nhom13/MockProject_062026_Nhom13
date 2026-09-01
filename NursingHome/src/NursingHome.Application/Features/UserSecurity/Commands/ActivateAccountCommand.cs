using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;
using NursingHome.Application.Features.UserSecurity.Validators;
namespace NursingHome.Application.Features.UserSecurity.Commands;


public class ActivateAccountCommand
    : IRequest<ApiResponse<ActivateAccountResponse>>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
}