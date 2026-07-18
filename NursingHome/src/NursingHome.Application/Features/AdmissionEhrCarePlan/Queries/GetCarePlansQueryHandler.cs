using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Abstractions.AdmissionEhrCarePlan;
using NursingHome.Application.Common.Models;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetCarePlansQueryHandler(
    ICarePlanRepository carePlanRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<GetCarePlansQuery, PageResult<CarePlanDto>>
{
    public async Task<PageResult<CarePlanDto>> Handle(GetCarePlansQuery request , CancellationToken cancellationToken)
    {
        long currentUserId = currentUserService.UserId ?? throw new UnauthorizedAccessException("Không xác định được danh tính người dùng (Token thiếu hoặc không hợp lệ).");

        var result =  await carePlanRepository.GetCarePlanListAsync(
            request.SearchTerm,
            request.Status,
            request.ReviewStatus,
            currentUserId,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;

    }
}