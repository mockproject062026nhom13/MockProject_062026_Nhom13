using MediatR;
using NursingHome.Application.Features.CnaDashboard.DTOs;

namespace NursingHome.Application.Features.CnaDashboard.Queries;

public record GetCnaDashboardQuery(long CnaUserId) : IRequest<CnaDashboardDto>;
