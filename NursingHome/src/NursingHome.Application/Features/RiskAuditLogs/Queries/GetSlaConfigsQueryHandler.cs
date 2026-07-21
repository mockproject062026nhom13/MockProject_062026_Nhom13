using MediatR;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public class GetSlaConfigsQueryHandler(ISlaConfigRepository repository)
    : IRequestHandler<GetSlaConfigsQuery, List<SlaConfigDto>>
{
    private readonly ISlaConfigRepository _repository = repository;

    public Task<List<SlaConfigDto>> Handle(GetSlaConfigsQuery request, CancellationToken cancellationToken)
        => _repository.GetAllAsync(cancellationToken);
}
