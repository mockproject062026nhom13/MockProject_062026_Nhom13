using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.LocationInfrastructure.Commands.UpdateLOCRate;

public class UpdateLOCRateCommandHandler
    : IRequestHandler<
        UpdateLOCRateCommand,
        ApiResponse<UpdateLOCRateResponse>>
{
    private readonly ILOCRateRepository _repository;

    public UpdateLOCRateCommandHandler(
        ILOCRateRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<UpdateLOCRateResponse>> Handle(
        UpdateLOCRateCommand request,
        CancellationToken cancellationToken)
    {
        // Mock userId
        // TODO: Get userId from JWT token
        //var userId = 1L;

        // TODO:
        // Do not validate facility access here.

        var careLevelRate = await _repository.GetLOCRateByIdAsync(
            request.Id,
            cancellationToken);

        if (careLevelRate == null)
        {
            return ApiResponse<UpdateLOCRateResponse>.CreateError(
                404,
                "LOC rate not found.");
        }

        // TODO:
        // Get old LOC rate for audit log.
        decimal oldRate = careLevelRate.DailyRate;

        // TODO:
        // Validate EffectiveFrom / EffectiveTo rule.
        // Validate overlap if required.

        careLevelRate.Update(
            request.CareLevelId,
            request.FacilityId,
            request.DailyRate,
            request.EffectiveFrom,
            request.EffectiveTo);

        await _repository.UpdateAsync(
            careLevelRate,
            cancellationToken);

        await _repository.SaveChangesAsync(
            cancellationToken);

        // TODO:
        // Create audit log.
        //
        // Action:
        // EDIT_LOC_RATE
        //
        // {
        //     facilityId,
        //     careLevelId,
        //     oldRate,
        //     newRate
        // }

        return ApiResponse<UpdateLOCRateResponse>.CreateSuccess(
            new UpdateLOCRateResponse
            {
                Id = careLevelRate.Id,
                FacilityId = careLevelRate.FacilityId,
                CareLevelId = careLevelRate.CareLevelId,
                DailyRate = careLevelRate.DailyRate,
                EffectiveFrom = careLevelRate.EffectiveFrom,
                EffectiveTo = careLevelRate.EffectiveTo
            },
            200,
            "LOC rate updated successfully.");
    }
}