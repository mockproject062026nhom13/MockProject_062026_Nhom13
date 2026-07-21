using MediatR;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

public class UpdateSlaConfigCommandHandler(ISlaConfigRepository repository)
    : IRequestHandler<UpdateSlaConfigCommand, bool>
{
    private readonly ISlaConfigRepository _repository = repository;

    public async Task<bool> Handle(UpdateSlaConfigCommand request, CancellationToken cancellationToken)
    {
        var isUpdated = await _repository.UpdateWindowAsync(request.Id, request.SlaWindowHrs, cancellationToken);

        if (!isUpdated)
            throw new NotFoundException($"Không tìm thấy cấu hình SLA có ID là {request.Id}.");

        return true;
    }
}
