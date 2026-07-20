using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetLocHistoryQueryHandler
    : IRequestHandler<GetLocHistoryQuery, ApiResponse<LocHistoryResultDto>>
{
    private readonly ILocClassificationRepository _locClassificationRepository;

    public GetLocHistoryQueryHandler(
        ILocClassificationRepository locClassificationRepository)
    {
        _locClassificationRepository = locClassificationRepository;
    }

    public async Task<ApiResponse<LocHistoryResultDto>> Handle(
        GetLocHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _locClassificationRepository.GetLocHistoryAsync(
            request.ResidentId,
            cancellationToken);

        if (result is null)
        {
            return ApiResponse<LocHistoryResultDto>.CreateError(
                404,
                "The requested resident was not found.");
        }

        return ApiResponse<LocHistoryResultDto>.CreateSuccess(result);
    }
}
