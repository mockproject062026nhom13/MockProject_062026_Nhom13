using NursingHome.Domain.Entities;
using NursingHome.Application.Features.CareLevelResidents.DTOs;
namespace NursingHome.Application.Abstractions;

public interface ICarePlanRepository
{
    Task<CarePlanInfoDto?> GetCarePlanInfoAsync(
        long residentId,
        CancellationToken cancellationToken);
    Task<List<long>> GetCarePlanIdByStatus(
        string status,
        CancellationToken cancellationToken);
    Task UpdateStatusByDON(
        long careplanId,
        string status,
        // string reason but db dont have this field :)
        CancellationToken cancellationToken
    );
    //  todo: need to see why need and not need this method
    // Task SaveChangesAsync(CancellationToken cancellationToken = default);
        
}