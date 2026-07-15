using NursingHome.Application.Features.RiskAuditLogs.Commands;

namespace NursingHome.Application.Abstractions;

public interface IIncidentRepository
{
    Task<long> CreateIncidentAsync(CreateIncidentCommand command, long currentUserId, CancellationToken cancellationToken);
}