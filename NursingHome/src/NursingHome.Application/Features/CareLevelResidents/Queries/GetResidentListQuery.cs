using MediatR;
using NursingHome.Application.Common; 
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Queries;

public record GetResidentListQuery(
    string? SearchTerm,
    string? Status,
    //string? Referral,
    string? PayerSource,
    int Page = 1,
    int PageSize = 10
) : IRequest<ApiResponse<IReadOnlyList<ResidentListDto>>>;