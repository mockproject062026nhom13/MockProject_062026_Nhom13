using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;
namespace NursingHome.Application.Features.LocationInfrastructure.Commands.CreateLOCRate;

public class CreateLOCRateCommand
    : IRequest<ApiResponse<CreateLOCRateResponse>>
{
    public long CareLevelId { get; set; }

    public long FacilityId { get; set; }

    public decimal DailyRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}