using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;
using NursingHome.Domain.Entities;

namespace NursingHome.Application.Features.LocationInfrastructure.Commands.CreateLOCRate;

public sealed class CreateLOCRateCommandHandler
    : IRequestHandler<
        CreateLOCRateCommand,
        ApiResponse<CreateLOCRateResponse>>
{
    private readonly ILOCRateRepository _locRateRepository;

    public CreateLOCRateCommandHandler(
        ILOCRateRepository locRateRepository)
    {
        _locRateRepository = locRateRepository;
    }

    public async Task<ApiResponse<CreateLOCRateResponse>> Handle(
        CreateLOCRateCommand request,
        CancellationToken cancellationToken)
    {
        // Mock userId
        // TODO: Get userId from JWT token
        //var userId = 1L;

        // TODO:
        // Check current user has permission for the facility.

        // TODO:
        // Validate overlapping effective date ranges.

        var careLevelRate = new CareLevelRate(
            request.CareLevelId,
            request.FacilityId,
            request.DailyRate,
            request.EffectiveFrom,
            request.EffectiveTo);

        await _locRateRepository.AddAsync(
            careLevelRate,
            cancellationToken);

        await _locRateRepository.SaveChangesAsync();

        // TODO:
        // Create audit log.

        return ApiResponse<CreateLOCRateResponse>.CreateSuccess(
            new CreateLOCRateResponse
            {
                FacilityId = careLevelRate.FacilityId,
                CareLevelId = careLevelRate.CareLevelId,
                DailyRate = careLevelRate.DailyRate,
                EffectiveFrom = careLevelRate.EffectiveFrom,
                EffectiveTo = careLevelRate.EffectiveTo
            },
            201,
            "LOC rate created successfully.");
    }
}