using MediatR;
using NursingHome.Application.Abstractions.CarePlans;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CarePlans.DTOs;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.CarePlans.Commands;

public class UpdateCarePlanCommandHandler(ICarePlanRepository repository)
    : IRequestHandler<UpdateCarePlanCommand, ApiResponse<CarePlanDetailDto>>
{
    private readonly ICarePlanRepository _repository = repository;

    public async Task<ApiResponse<CarePlanDetailDto>> Handle(UpdateCarePlanCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await _repository.GetSnapshotAsync(request.Id, cancellationToken);

        if (snapshot is null || snapshot.IsDeleted)
            throw new NotFoundException($"Không tìm thấy care plan có ID là {request.Id}.");

        if (snapshot.Status != "DRAFT")
            throw new DomainException("Chỉ có thể sửa care plan ở trạng thái DRAFT.");

        var result = await _repository.ReplaceCareAreasAsync(request.Id, request.CareAreas, cancellationToken);

        return ApiResponse<CarePlanDetailDto>.CreateSuccess(result, 200, "Cập nhật care plan thành công.");
    }
}
