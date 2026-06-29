namespace CRUDAccountDemo.API.Common;

public record PaginationInfo(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalItems,
    bool HasNext,
    bool HasPrevious);
