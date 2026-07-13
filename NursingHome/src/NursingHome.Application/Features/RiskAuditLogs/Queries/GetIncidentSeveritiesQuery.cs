using MediatR;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public record GetIncidentSeveritiesQuery(): IRequest<List<IncidentSeverityDto>>;

