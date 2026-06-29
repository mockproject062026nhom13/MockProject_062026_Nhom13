namespace CRUDAccountDemo.Business.DTOs;

public record PagedResult<T>(
    IEnumerable<T> Items,
    int Page,
    int PageSize,
    int TotalItems);
