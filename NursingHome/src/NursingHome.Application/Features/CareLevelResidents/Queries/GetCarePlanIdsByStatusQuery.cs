using MediatR;
using NursingHome.Application.Common; 
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Queries;
public sealed record GetCarePlanIdsByStatusQuery(
    string Status
) : IRequest<ApiResponse<List<long>>>;