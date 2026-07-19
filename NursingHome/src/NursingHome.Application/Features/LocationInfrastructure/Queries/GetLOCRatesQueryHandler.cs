using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;


namespace NursingHome.Application.Features.LocationInfrastructure.Queries.GetLOCRates;

public sealed class GetLOCRatesQueryHandler
    : IRequestHandler<
        GetLOCRatesQuery,
        ApiResponse<List<GetLOCRateDto>>>
{
    private readonly ILOCRateRepository _locRateRepository;

    public GetLOCRatesQueryHandler(
        ILOCRateRepository locRateRepository)
    {
        _locRateRepository = locRateRepository;
    }

    public async Task<ApiResponse<List<GetLOCRateDto>>> Handle(
        GetLOCRatesQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: Get userId from JWT token
        // TODO:
        // Check current user has permission for facility access.


        var locRateDtos = new List<GetLOCRateDto>();

        if (request.LOCRateId.HasValue)
        {
            var locRate = await _locRateRepository.GetLOCRateByIdAsync(
                request.LOCRateId.Value,
                cancellationToken);

            if (locRate != null)
            {
                locRateDtos.Add(new GetLOCRateDto
                {
                    Id = locRate.Id,
                    FacilityId = locRate.FacilityId,
                    CareLevelId = locRate.CareLevelId,
                    DailyRate = locRate.DailyRate,
                    EffectiveFrom = locRate.EffectiveFrom,
                    EffectiveTo = locRate.EffectiveTo
                });
            }
        }
        else
        {
            var locRates = await _locRateRepository.GetLOCRatesAsync(
                request.FromDate,
                request.ToDate,
                cancellationToken);

            locRateDtos = locRates
                .Select(x => new GetLOCRateDto
                {
                    Id = x.Id,
                    FacilityId = x.FacilityId,
                    CareLevelId = x.CareLevelId,
                    DailyRate = x.DailyRate,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo
                })
                .ToList();
        }

        return ApiResponse<List<GetLOCRateDto>>.CreateSuccess(
            locRateDtos,
            200,
            "LOC rates retrieved successfully.");
    }
}