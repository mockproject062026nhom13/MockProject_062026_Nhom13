using MediatR;
using System.Threading;
using System.Threading.Tasks;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Application.Features.CnaDashboard.DTOs;

namespace NursingHome.Application.Features.CnaDashboard.Queries;

public class GetCnaDashboardQueryHandler : IRequestHandler<GetCnaDashboardQuery, CnaDashboardDto>
{
    private readonly ICnaDashboardService _cnaDashboardService;

    public GetCnaDashboardQueryHandler(ICnaDashboardService cnaDashboardService)
    {
        _cnaDashboardService = cnaDashboardService;
    }

    public async Task<CnaDashboardDto> Handle(GetCnaDashboardQuery request, CancellationToken cancellationToken)
    {
        return await _cnaDashboardService.GetDashboardAsync(request.CnaUserId, cancellationToken);
    }
}
