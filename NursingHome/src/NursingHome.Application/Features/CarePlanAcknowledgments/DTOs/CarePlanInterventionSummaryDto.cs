namespace NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

public class CarePlanInterventionSummaryDto
{
    public long InterventionId { get; init; }

    public string AssignedRole { get; init; } = string.Empty;
}
