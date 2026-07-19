using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Abstractions;

namespace NursingHome.Application.Features.Beds.GetBeds;

public class GetBedsQueryHandler(IBedRepository repository)
    : IRequestHandler<GetBedsQuery, ApiResponse<List<BedRecordDto>>>
{
    public async Task<ApiResponse<List<BedRecordDto>>> Handle(
        GetBedsQuery request,
        CancellationToken ct)
    {
        return await repository.GetBedsAsync(request.Page, ct);
    }
}
