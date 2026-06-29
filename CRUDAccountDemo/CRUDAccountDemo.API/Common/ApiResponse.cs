namespace CRUDAccountDemo.API.Common;

public record ApiResponse<T>(
    bool Success,
    int StatusCode,
    string Message,
    T? Data,
    IEnumerable<ErrorDetail>? Errors,
    PaginationInfo? Pagination);
