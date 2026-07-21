using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CarePlans.DTOs;

namespace NursingHome.Application.Features.CarePlans.Commands;

public record ActivateCarePlanCommand(long Id) : IRequest<ApiResponse<CarePlanActivationResultDto>>;
