using FastEndpoints;

namespace NursingHome.Api.Features.SC007;

public class TestConnection : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/test");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await HttpContext.Response.WriteAsync("success", ct);
    }
}