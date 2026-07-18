using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Abstractions.AdmissionEhrCarePlan;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetCarePlanStatisticsQueryHandler(
    ICarePlanRepository carePlanRepository,
    ICurrentUserService currentUserService):IRequestHandler<GetCarePlanStatisticsQuery , CarePlanStatisticDto>
{
    public async Task<CarePlanStatisticDto> Handle(GetCarePlanStatisticsQuery request , CancellationToken cancellationToken)
    {
        var currentNurseId = currentUserService.UserId;

        var result = await carePlanRepository.GetSratisticsByNurseIdAsync(currentNurseId,cancellationToken);
        return result;
    }
}