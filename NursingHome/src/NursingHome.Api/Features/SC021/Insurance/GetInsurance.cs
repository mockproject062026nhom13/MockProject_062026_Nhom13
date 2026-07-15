using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Api.Features.Insurance;

public class GetAllInsuranceEndpoint(NursingHomeDbContext db)
    : EndpointWithoutRequest<ApiResponse<List<InsuranceProvider>>>
{
    public override void Configure()
    {
        Get("/api/insurance");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var providers = await db.InsuranceProviders.ToListAsync(ct);

        await SendAsync(ApiResponse<List<InsuranceProvider>>.CreateSuccess(providers), 200, ct);
    }
}
