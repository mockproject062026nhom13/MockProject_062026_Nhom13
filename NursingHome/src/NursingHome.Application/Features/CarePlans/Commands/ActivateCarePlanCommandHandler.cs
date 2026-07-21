using MediatR;
using NursingHome.Application.Abstractions.CarePlans;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CarePlans.DTOs;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.CarePlans.Commands;

public class ActivateCarePlanCommandHandler(ICarePlanRepository repository)
    : IRequestHandler<ActivateCarePlanCommand, ApiResponse<CarePlanActivationResultDto>>
{
    private readonly ICarePlanRepository _repository = repository;

    public async Task<ApiResponse<CarePlanActivationResultDto>> Handle(ActivateCarePlanCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await _repository.GetSnapshotAsync(request.Id, cancellationToken);

        if (snapshot is null || snapshot.IsDeleted)
            throw new NotFoundException($"Không tìm thấy care plan có ID là {request.Id}.");

        if (snapshot.Status != "DRAFT")
            throw new DomainException("Chỉ kích hoạt được care plan ở trạng thái DRAFT.");

        if (snapshot.InterventionCount == 0)
            throw new DomainException("Care plan cần ít nhất 1 can thiệp (intervention) để sinh công việc.");

        var tasksGenerated = await _repository.ActivateAsync(request.Id, cancellationToken);

        var data = new CarePlanActivationResultDto(request.Id, "ACTIVE", tasksGenerated);
        return ApiResponse<CarePlanActivationResultDto>.CreateSuccess(data, 200, "Kích hoạt care plan thành công.");
    }
}
