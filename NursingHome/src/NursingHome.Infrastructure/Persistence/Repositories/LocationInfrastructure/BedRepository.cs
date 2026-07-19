using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories;

public class BedRepository(NursingHomeDbContext db) : IBedRepository
{
    public async Task<ApiResponse<List<BedRecordDto>>> GetBedsAsync(int page, CancellationToken ct)
    {
        const int pageSize = 4;

        var totalItems = await db.Beds.CountAsync(ct);
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var data = await db.Beds
            .OrderBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BedRecordDto(
                b.Room.RoomNumber,
                b.Room.RoomType,
                b.BedNumber,
                b.Status))
            .ToListAsync(ct);

        var pagination = new PaginationMetadata(
            Page: page,
            PageSize: pageSize,
            TotalPages: totalPages,
            TotalItems: totalItems,
            HasNext: page < totalPages,
            HasPrevious: page > 1);

        return ApiResponse<List<BedRecordDto>>.CreateSuccess(data, pagination: pagination);
    }
}
