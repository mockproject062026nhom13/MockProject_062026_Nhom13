using MediatR;
using NursingHome.Application.Common; 

namespace NursingHome.Application.Features.CareLevelResidents.Queries;

public record ResidentStatisticsDto(
    int TotalResidents, 
    int Active, 
    int Discharged, 
    int Pending,
    int Deceased
);

public record GetResidentStatisticsQuery() : IRequest<ApiResponse<ResidentStatisticsDto>>;