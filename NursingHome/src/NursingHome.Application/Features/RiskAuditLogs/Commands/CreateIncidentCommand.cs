using MediatR;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

public record CreateIncidentCommand(
    string IncidentType,            
    long SeverityId,                
    long ResidentId,                
    string Location,                
    string Description,    
    string? Witness,         
    string ImmediateActionsTaken,  
    DateTimeOffset ReportedAt 
) : IRequest<ApiResponse<long>>;