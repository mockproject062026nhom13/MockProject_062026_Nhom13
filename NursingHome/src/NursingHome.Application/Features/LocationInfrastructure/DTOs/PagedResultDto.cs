namespace NursingHome.Application.Features.LocationInfrastructure.DTOs;

public class PagedResultDto<T>
{
    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }

    public IReadOnlyList<T> Items { get; init; } = [];
}
