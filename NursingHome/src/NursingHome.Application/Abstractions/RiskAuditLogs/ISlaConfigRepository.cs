using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Abstractions.RiskAuditLogs;

public interface ISlaConfigRepository
{
    Task<List<SlaConfigDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateWindowAsync(long id, int slaWindowHrs, CancellationToken cancellationToken = default);
}
