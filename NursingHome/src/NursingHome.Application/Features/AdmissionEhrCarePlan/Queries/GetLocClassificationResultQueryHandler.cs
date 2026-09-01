using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetLocClassificationResultQueryHandler
    : IRequestHandler<GetLocClassificationResultQuery, ApiResponse<LocClassificationResultDto>>
{
    private readonly ILocClassificationRepository _locClassificationRepository;

    public GetLocClassificationResultQueryHandler(
        ILocClassificationRepository locClassificationRepository)
    {
        _locClassificationRepository = locClassificationRepository;
    }

    public async Task<ApiResponse<LocClassificationResultDto>> Handle(
        GetLocClassificationResultQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _locClassificationRepository.GetLocClassificationResultAsync(
            request.AssessmentId,
            cancellationToken);

        if (result is null)
        {
            return ApiResponse<LocClassificationResultDto>.CreateError(
                404,
                "The requested assessment was not found.");
        }

        return ApiResponse<LocClassificationResultDto>.CreateSuccess(result);
    }
}
