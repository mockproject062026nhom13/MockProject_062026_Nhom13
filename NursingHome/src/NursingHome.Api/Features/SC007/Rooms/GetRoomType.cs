using FastEndpoints;
using NursingHome.Domain.Constant;
using NursingHome.Application.Common;

public class GetType : EndpointWithoutRequest<ApiResponse<List<string>>>
{
    public override void Configure()
    {
        Get("api/roomtype");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var data = RoomConstants.RoomTypes.ToList();
        var response = ApiResponse<List<string>>.CreateSuccess(data);
        await SendAsync(response, cancellation: ct);
    }
}
