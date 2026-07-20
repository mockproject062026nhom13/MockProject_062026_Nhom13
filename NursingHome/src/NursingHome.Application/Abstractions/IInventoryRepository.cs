using System.Threading;
using System.Threading.Tasks;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;
using NursingHome.Application.Features.LocationInfrastructure.Queries;

namespace NursingHome.Application.Abstractions;

public interface IInventoryRepository
{
    Task<EquipmentSupplyInventoryDto> GetEquipmentSupplyInventoryAsync(
        GetEquipmentSupplyInventoryQuery query,
        CancellationToken cancellationToken);
}
