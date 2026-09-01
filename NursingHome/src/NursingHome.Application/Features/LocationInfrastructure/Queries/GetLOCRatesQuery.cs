using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;

namespace NursingHome.Application.Features.LocationInfrastructure.Queries.GetLOCRates;

public sealed record GetLOCRatesQuery(
    long? LOCRateId,
    DateOnly? FromDate,
    DateOnly? ToDate)
    : IRequest<ApiResponse<List<GetLOCRateDto>>>;