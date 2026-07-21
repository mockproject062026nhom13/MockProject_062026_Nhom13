using MediatR;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.UserSecurity.Commands;

// Action ∈ DEACTIVATE (soft-delete, AD-04) | REACTIVATE | SUSPEND.
public record ChangeUserStatusCommand(long Id, string Action) : IRequest<ApiResponse<bool>>;
