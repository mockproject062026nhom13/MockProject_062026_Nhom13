using NursingHome.Application.Features.CareLevelResidents.Queries;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Abstractions;

public interface ICareLevelResidentRepository
{
    Task<(IReadOnlyList<ResidentListDto> Items, int TotalCount)> GetResidentListAsync(
        string? searchTerm, 
        string? status, 
        string? payerSource,
        int page, 
        int pageSize, 
        CancellationToken cancellationToken);
        
    Task<ResidentStatisticsDto> GetResidentStatisticsAsync(CancellationToken cancellationToken);
}