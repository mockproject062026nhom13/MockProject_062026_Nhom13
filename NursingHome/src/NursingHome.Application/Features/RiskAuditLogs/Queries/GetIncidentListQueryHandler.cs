using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public class GetIncidentListQueryHandler : IRequestHandler<GetIncidentListQuery, List<IncidentListItemDto>>
{
    private readonly IIncidentRepository _repository;
    public GetIncidentListQueryHandler(IIncidentRepository repository)
    {
        _repository = repository;

    }
    public async Task<List<IncidentListItemDto>> Handle(GetIncidentListQuery request, CancellationToken cancellationToken)
    {
        int skipAmount = (request.PageNumber -1 )*request.PageSize;

        return await _repository.GetPagedIncidentsAsync(
            request.StatusFilter,
            request.SeverityFilter,
            skipAmount,
            request.PageSize,
            cancellationToken
        );
    }
}