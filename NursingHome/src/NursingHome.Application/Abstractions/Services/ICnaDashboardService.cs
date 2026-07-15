using System.Threading;
using System.Threading.Tasks;
using NursingHome.Application.Features.CnaDashboard.DTOs;

namespace NursingHome.Application.Abstractions.Services;

public interface ICnaDashboardService
{
    Task<CnaDashboardDto> GetDashboardAsync(long cnaUserId, CancellationToken cancellationToken = default);
    Task CompleteTaskAsync(long taskId, long cnaUserId, CancellationToken cancellationToken = default);
}
