using MediatR;
using NursingHome.Application.Abstractions.CarePlans;
using NursingHome.Application.Common;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.CarePlans.Commands;

public class SubmitCarePlanCommandHandler(ICarePlanRepository repository)
    : IRequestHandler<SubmitCarePlanCommand, ApiResponse<object>>
{
    private readonly ICarePlanRepository _repository = repository;

    public async Task<ApiResponse<object>> Handle(SubmitCarePlanCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await _repository.GetSnapshotAsync(request.Id, cancellationToken);

        if (snapshot is null || snapshot.IsDeleted)
            throw new NotFoundException($"Không tìm thấy care plan có ID là {request.Id}.");

        if (snapshot.Status != "DRAFT")
            throw new DomainException("Chỉ gửi duyệt được care plan ở trạng thái DRAFT.");

        if (snapshot.GoalCount == 0)
            throw new DomainException("Care plan cần ít nhất 1 care area trước khi gửi duyệt.");

        // "Pending Review" là mô phỏng — schema care_plans chỉ có DRAFT/ACTIVE/RESOLVED/DISCONTINUED,
        // không có trạng thái này nên KHÔNG persist (giữ DRAFT).
        return ApiResponse<object>.CreateSuccess(
            data: null,
            statusCode: 200,
            message: "Đã gửi DON duyệt (Pending Review — mô phỏng, chưa lưu do schema không có trạng thái này).");
    }
}
