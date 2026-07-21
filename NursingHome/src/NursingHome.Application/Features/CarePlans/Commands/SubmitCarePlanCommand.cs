using MediatR;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.CarePlans.Commands;

public record SubmitCarePlanCommand(long Id) : IRequest<ApiResponse<object>>;
