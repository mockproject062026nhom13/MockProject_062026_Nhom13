using MediatR;
using NursingHome.Application.Abstractions.CarePlans;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CarePlans.DTOs;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.CarePlans.Commands;

public class CreateCarePlanCommandHandler(ICarePlanRepository repository)
    : IRequestHandler<CreateCarePlanCommand, ApiResponse<CarePlanDetailDto>>
{
    private readonly ICarePlanRepository _repository = repository;

    public async Task<ApiResponse<CarePlanDetailDto>> Handle(CreateCarePlanCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ResidentExistsAsync(request.ResidentId, cancellationToken))
            throw new NotFoundException($"Không tìm thấy cư dân có ID là {request.ResidentId}.");

        if (await _repository.IsResidentChartLockedAsync(request.ResidentId, cancellationToken))
            throw new DomainException("Hồ sơ cư dân đang bị khóa (chart lock) — không thể tạo care plan.");

        var result = await _repository.CreateDraftAsync(
            request.ResidentId, request.SignificantChangeFlag, request.CareAreas, cancellationToken);

        return ApiResponse<CarePlanDetailDto>.CreateSuccess(result, 201, "Tạo care plan (nháp) thành công.");
    }
}
