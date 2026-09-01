using MediatR;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public class GetIncidentSeveritiesQueryHandler(IIncidentSeverityRepository repository)
    : IRequestHandler<GetIncidentSeveritiesQuery, List<IncidentSeverityDto>>
{
    private readonly IIncidentSeverityRepository _repository = repository;

    public async Task<List<IncidentSeverityDto>> Handle(GetIncidentSeveritiesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}