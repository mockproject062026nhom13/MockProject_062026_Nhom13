using MediatR;
using NursingHome.Application.Common; 
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Queries;

public record GetResidentStatisticsQuery() : IRequest<ApiResponse<ResidentStatisticsDto>>;