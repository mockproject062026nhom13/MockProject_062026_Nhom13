using NursingHome.Application.Features.RiskAuditLogs.DTOs;


namespace NursingHome.Application.Abstractions.RiskAuditLogs;
public interface IIncidentSeverityRepository
{
    Task<List<IncidentSeverityDto>> GetAllAsync();
    Task<bool> UpdateDescriptionAndExampleAsync(long id, string? description, string? example);
}