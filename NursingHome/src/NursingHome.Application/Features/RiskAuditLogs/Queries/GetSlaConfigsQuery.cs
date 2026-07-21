using MediatR;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public record GetSlaConfigsQuery() : IRequest<List<SlaConfigDto>>;
