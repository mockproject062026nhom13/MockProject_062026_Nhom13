using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CarePlans.DTOs;

namespace NursingHome.Application.Features.CarePlans.Commands;

public record CreateCarePlanCommand(
    long ResidentId,
    bool SignificantChangeFlag,
    List<CareAreaInput> CareAreas
) : IRequest<ApiResponse<CarePlanDetailDto>>;
