using System;
using MediatR;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.UserSecurity.Commands;

public class CreateUserCommand : IRequest<ApiResponse<Guid>>
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; } 
    public long RoleId { get; set; }
    public Guid? AssignedFacilityId { get; set; }
}