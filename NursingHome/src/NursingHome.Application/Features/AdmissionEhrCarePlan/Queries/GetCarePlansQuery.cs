using MediatR;
using NursingHome.Application.Common.Models;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public record GetCarePlansQuery(
    string? SearchTerm,
    string? Status,
    string? ReviewStatus,
    int PageIndex = 1,
    int PageSize = 10
): IRequest<PageResult<CarePlanDto>>;