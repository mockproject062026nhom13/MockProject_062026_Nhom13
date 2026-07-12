using MediatR;
using NursingHome.Application.Common; 

namespace NursingHome.Application.Features.CareLevelResidents.Queries;

public record ResidentListDto(
    long Id,
    string FullName,
    string DobWithAge, // return with format "yyyy-MM-dd (Age)"
    string Status,
    string Room,// return with format "room-bed"
    //string? ReferralSource,
    string? PayerSource
);

public record GetResidentListQuery(
    string? SearchTerm,
    string? Status,
    //string? Referral,
    string? PayerSource,
    int Page = 1,
    int PageSize = 10
) : IRequest<ApiResponse<IReadOnlyList<ResidentListDto>>>;