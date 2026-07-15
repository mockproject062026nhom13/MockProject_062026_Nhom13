using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public class GetIncidentSummaryQueryHandler:IRequestHandler<GetIncidentSummaryQuery,IncidentSummaryDto>
{
    private readonly IIncidentRepository _repository;
    //constructor
    public GetIncidentSummaryQueryHandler(IIncidentRepository repository)
    {
        _repository = repository;
    } 
    public async Task<IncidentSummaryDto> Handle(GetIncidentSummaryQuery request,CancellationToken cancellationToken)
    { 
        return await _repository.GetIncidentSummaryAsync(
            request.Month,
            request.Year,
            cancellationToken
        );
    }

}