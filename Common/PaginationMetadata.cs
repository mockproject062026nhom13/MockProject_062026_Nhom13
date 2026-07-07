namespace Models.Common;

public record PaginationMetadata(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalItems,
    bool HasNext,
    bool HasPrevious);
