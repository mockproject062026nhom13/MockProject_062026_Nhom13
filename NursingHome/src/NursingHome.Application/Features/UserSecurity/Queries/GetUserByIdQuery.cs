using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;

namespace NursingHome.Application.Features.UserSecurity.Queries;

public record GetUserByIdQuery(long Id) : IRequest<ApiResponse<UserDetailDto>>;
