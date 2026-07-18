using MediatR;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public record GetCarePlanStatisticsQuery : IRequest<CarePlanStatisticDto>;