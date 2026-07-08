using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;
using NursingHome.Application.Common;

public class GetAllResidentsEndpoint(NursingHomeDbContext db) :
    EndpointWithoutRequest<ApiResponse<List<resident>>>
{
    public override void Configure()
    {
        Get("api/residents");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var residents = await db.residents
            .ToListAsync(ct);

        var response = ApiResponse<List<resident>>.CreateSuccess(residents);
        await SendAsync(response, cancellation: ct);
    }
}
