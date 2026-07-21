using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Abstractions.RiskAuditLogs;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

public class CreateIncidentCommandHandler(IIncidentRepository _incidentRepository) 
    : IRequestHandler<CreateIncidentCommand, ApiResponse<long>>
{
    public async Task<ApiResponse<long>> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        // hardcode UserId
        long currentUserId = 5; 

        var incidentId = await _incidentRepository.CreateIncidentAsync(request, currentUserId, cancellationToken);

        if (incidentId <= 0)
        {
            return ApiResponse<long>.CreateError(400, "Failed to submit report. Invalid resident or severity data.");
        }

        return ApiResponse<long>.CreateSuccess(
            data: incidentId,
            statusCode: 201,
            message: "Incident reported successfully. SLA deadline has been set and timeline updated."
        );
    }
}