using System.Threading;
using System.Threading.Tasks;

namespace NursingHome.Application.Abstractions.BillingInsurance;

public interface ICareCostEstimateRepository
{
    Task<CareCostDataDto?> GetCareCostDataAsync(long carePlanId, CancellationToken cancellationToken = default);
}