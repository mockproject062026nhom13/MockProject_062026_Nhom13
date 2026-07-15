using MediatR;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.Beds.GetBeds;

public record GetBedsQuery(int Page = 1)
    : IRequest<ApiResponse<List<BedRecordDto>>>;
