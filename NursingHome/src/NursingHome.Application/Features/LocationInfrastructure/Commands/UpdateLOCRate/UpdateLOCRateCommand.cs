using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;
namespace NursingHome.Application.Features.LocationInfrastructure.Commands.UpdateLOCRate;

public class UpdateLOCRateCommand
    : IRequest<ApiResponse<UpdateLOCRateResponse>>
{
    public long Id { get; set; }

    public long CareLevelId { get; set; }

    public long FacilityId { get; set; }

    public decimal DailyRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}