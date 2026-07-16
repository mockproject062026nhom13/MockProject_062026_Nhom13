using NursingHome.Application.Common;

namespace NursingHome.Application.Abstractions;

public interface IBedRepository
{
    Task<ApiResponse<List<BedRecordDto>>> GetBedsAsync(int page, CancellationToken ct);
}
