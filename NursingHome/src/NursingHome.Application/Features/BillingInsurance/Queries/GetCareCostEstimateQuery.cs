using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.BillingInsurance.DTOs;

namespace NursingHome.Application.Features.BillingInsurance.Queries;

public record GetCareCostEstimateQuery(long CarePlanId) : IRequest<ApiResponse<CareCostEstimateDto>>;