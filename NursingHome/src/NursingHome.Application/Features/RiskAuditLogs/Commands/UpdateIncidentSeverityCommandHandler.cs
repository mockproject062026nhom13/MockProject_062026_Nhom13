using MediatR;
using NursingHome.Application.Abstractions.RiskAuditLogs;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

public class UpdateIncidentSeverityCommandHandler(IIncidentSeverityRepository repository)
    : IRequestHandler<UpdateIncidentSeverityCommand,bool>
{
    private readonly IIncidentSeverityRepository _repository = repository;

    public async Task<bool> Handle(UpdateIncidentSeverityCommand request, CancellationToken cancellationToken)
    {
        bool isUpdate = await _repository.UpdateDescriptionAndExampleAsync(
            request.Id,
            request.Description,
            request.Example
        );

        if(!isUpdate)
            throw new KeyNotFoundException($"Không tìm thấy Cấp độ sự cố có ID là {request.Id}");

        return true;
    }

}