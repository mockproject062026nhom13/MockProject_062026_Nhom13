using FastEndpoints;
using NursingHome.Domain.Constant;
using NursingHome.Application.Common;

public class GetRequiredConsentsEndpoint :
    EndpointWithoutRequest<ApiResponse<List<string>>>
{
    public override void Configure()
    {
        Get("api/consents/required");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = ApiResponse<List<string>>.CreateSuccess(ConsentConstants.RequiredConsents);
        await SendAsync(response, cancellation: ct);
    }
}
