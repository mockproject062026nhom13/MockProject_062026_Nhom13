using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Abstractions.BillingInsurance;
using NursingHome.Application.Features.BillingInsurance.DTOs;
using NursingHome.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NursingHome.Application.Features.BillingInsurance.Queries;

public class GetCareCostEstimateQueryHandler(ICareCostEstimateRepository repository) 
    : IRequestHandler<GetCareCostEstimateQuery, ApiResponse<CareCostEstimateDto>>
{
    //Khai báo trước các trường để hardcode
    const decimal PrivateRoomRate = 200.00m;
    const decimal SemiPrivateRoomRate = 140.00m;
    const decimal MedicationRate = 45.00m;

    const int EstimatedDaysPerMonth = 30;//ước tính theo tháng = 30 ngày
    const int MedicareDaysUsed = 45;//số ngày đã sử dụng trong Medicare Part A
    const int MedicareTotalDays = 100;
    int remainingDays = MedicareTotalDays - MedicareDaysUsed;

    public async Task<ApiResponse<CareCostEstimateDto>> Handle(GetCareCostEstimateQuery request, CancellationToken cancellationToken)
    {
        var careCostData = await repository.GetCareCostDataAsync(request.CarePlanId, cancellationToken);

        if (careCostData == null)
            throw new DomainException($"Care Plan with ID {request.CarePlanId} not found.");

        decimal locRate = careCostData.LocDailyRate;
        string locRateSource = careCostData.CareLevelRateId.HasValue ? $"AD-{careCostData.CareLevelRateId.Value:D2}" : "N/A";
        

        // TODO: Hardcode vì database chưa có bảng giá phòng (RoomTypeRates) và giá thuốc (MedicationCatalogs)

        // Hardcode roomrate/day
        decimal roomRate = (careCostData.RoomType ?? string.Empty).ToUpperInvariant() switch
        {
            "PRIVATE" => PrivateRoomRate,
            "SEMI_PRIVATE" => SemiPrivateRoomRate,
            _ => SemiPrivateRoomRate
        };

        // Hardcode medication/day
        decimal medicationRate = MedicationRate;

        var breakdowns = new List<CostBreakdownDto>
        {
            new CostBreakdownDto("LOC Daily Rate", locRate, $"Source: LOC Rate Table ({locRateSource})"),
            new CostBreakdownDto("Room Rate", roomRate, $"Source: Standard {careCostData.RoomType ?? "Unassigned"} Rate (Hardcoded)"),
            new CostBreakdownDto("Medication (est.)", medicationRate, "Source: Pharmacy - Estimated Average (Hardcoded)")
        };

        decimal totalDaily = locRate + roomRate + medicationRate;

        // Ghép fullname
        string fullName = $"{careCostData.ResidentFirstName} {careCostData.ResidentMiddleName} {careCostData.ResidentLastName}".Replace("  ", " ").Trim();

        var dto = new CareCostEstimateDto(
            ResidentName: fullName,
            RoomNumber: careCostData.RoomNumber,
            RoomType: careCostData.RoomType,
            CarePlanId: careCostData.CarePlanId,
            CarePlanStatus: careCostData.CarePlanStatus,
            LocTier: careCostData.CareLevelName,
            DailyEstimate: totalDaily,
            MonthlyEstimate: totalDaily * EstimatedDaysPerMonth, // Chi phí ước tính theo tháng
            MedicareDaysUsed: MedicareDaysUsed, //Hardcode số ngày Resident đã sử dụng bảo hiểm Medicare Part A
            MedicareTotalDays: MedicareTotalDays,
            MedicareAlert: $"Medicare days will expire in {remainingDays} days.",
            Breakdowns: breakdowns.AsReadOnly()
        );

        return ApiResponse<CareCostEstimateDto>.CreateSuccess(dto);
    }
}