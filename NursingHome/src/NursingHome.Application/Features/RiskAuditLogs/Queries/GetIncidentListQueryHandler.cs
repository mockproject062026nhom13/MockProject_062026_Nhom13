using MediatR;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Application.Common.Models;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public class GetIncidentListQueryHandler : IRequestHandler<GetIncidentListQuery, PageResult<IncidentListItemDto>>
{
    private readonly IIncidentRepository _repository;
    public GetIncidentListQueryHandler(IIncidentRepository repository)
    {
        _repository = repository;

    }
    public async Task<PageResult<IncidentListItemDto>> Handle(GetIncidentListQuery request, CancellationToken cancellationToken)
    {
        //int skipAmount = (request.PageNumber -1 )*request.PageSize;

        return await _repository.GetPagedIncidentsAsync(
            request.StatusFilter,
            request.SeverityFilter,
            request.PageNumber,
            request.PageSize,
            cancellationToken
        );
    }
}