using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Abstractions;

namespace NursingHome.Application.Features.CareLevelResidents.Queries;

public class GetResidentListQueryHandler(ICareLevelResidentRepository _repository) 
    : IRequestHandler<GetResidentListQuery, ApiResponse<IReadOnlyList<ResidentListDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<ResidentListDto>>> Handle(GetResidentListQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetResidentListAsync(
            request.SearchTerm, 
            request.Status, 
            request.PayerSource,
            request.Page, 
            request.PageSize, 
            cancellationToken);

        var totalPages = result.TotalCount > 0 ? (int)Math.Ceiling(result.TotalCount / (double)request.PageSize) : 0;
        
        var pagination = new PaginationMetadata(
            Page: request.Page,
            PageSize: request.PageSize,
            TotalPages: totalPages,
            TotalItems: result.TotalCount,
            HasNext: request.Page < totalPages,
            HasPrevious: request.Page > 1
        );

        return ApiResponse<IReadOnlyList<ResidentListDto>>.CreateSuccess(
            data: result.Items,
            statusCode: 200,
            message: "Resident list retrieved successfully.",
            pagination: pagination
        );
    }
}