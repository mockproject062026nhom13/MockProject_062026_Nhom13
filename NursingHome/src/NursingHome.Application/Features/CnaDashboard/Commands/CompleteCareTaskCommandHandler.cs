using MediatR;
using System.Threading;
using System.Threading.Tasks;
using NursingHome.Application.Abstractions.Services;

namespace NursingHome.Application.Features.CnaDashboard.Commands;

public class CompleteCareTaskCommandHandler : IRequestHandler<CompleteCareTaskCommand, Unit>
{
    private readonly ICnaDashboardService _cnaDashboardService;

    public CompleteCareTaskCommandHandler(ICnaDashboardService cnaDashboardService)
    {
        _cnaDashboardService = cnaDashboardService;
    }

    public async Task<Unit> Handle(CompleteCareTaskCommand request, CancellationToken cancellationToken)
    {
        await _cnaDashboardService.CompleteTaskAsync(request.TaskId, request.CnaUserId, cancellationToken);
        return Unit.Value;
    }
}
