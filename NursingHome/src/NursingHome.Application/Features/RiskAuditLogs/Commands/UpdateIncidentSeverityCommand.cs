using MediatR;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

public record UpdateIncidentSeverityCommand(long Id, string? Description, string? Example) : IRequest<bool>;