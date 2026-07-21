using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;

namespace NursingHome.Application.Features.UserSecurity.Queries;

public record GetUserStatisticsQuery() : IRequest<ApiResponse<UserStatisticsDto>>;
