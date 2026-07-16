// using FastEndpoints;
// using Microsoft.EntityFrameworkCore;
// using NursingHome.Application.Common;
// using NursingHome.Infrastructure.Persistence.DbContexts;

// public record GetBedsRequest(int Page = 1);

// public class GetBedsEndpoint(NursingHomeDbContext db) : Endpoint<GetBedsRequest, ApiResponse<List<BedRecord>>>
// {
//     public override void Configure()
//     {
//         Get("/api/beds");
//         AllowAnonymous();
//     }

//     public override async Task HandleAsync(GetBedsRequest req, CancellationToken ct)
//     {
//         int pageSize = 4;
//         int totalItems = await db.Beds.CountAsync(ct);
//         int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

//         var data = await db.Beds
//             .OrderBy(b => b.Id)
//             .Skip((req.Page - 1) * pageSize)
//             .Take(pageSize)
//             .Select(b => new BedRecord(
//                 b.Room.RoomNumber,
//                 b.Room.RoomType,
//                 b.BedNumber,
//                 b.Status
//             ))
//             .ToListAsync(ct);

//         var pagination = new PaginationMetadata(
//             Page: req.Page,
//             PageSize: pageSize,
//             TotalPages: totalPages,
//             TotalItems: totalItems,
//             HasNext: req.Page < totalPages,
//             HasPrevious: req.Page > 1
//         );

//         var response = ApiResponse<List<BedRecord>>.CreateSuccess(data, pagination: pagination);
//         await SendAsync(response, cancellation: ct);
//     }
// }
