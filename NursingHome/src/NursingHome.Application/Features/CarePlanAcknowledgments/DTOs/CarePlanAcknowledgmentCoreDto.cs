namespace NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

public class CarePlanAcknowledgmentCoreDto
{
    public CarePlanAcknowledgmentSummaryDto CarePlan { get; init; } = null!;

    public CarePlanAcknowledgmentResidentDto Resident { get; init; } = null!;

    public IReadOnlyList<CarePlanGoalSummaryDto> Goals { get; init; } = [];

    public IReadOnlyList<CarePlanInterventionSummaryDto> Interventions { get; init; } = [];

    public IReadOnlyList<CarePlanTaskSummaryDto> Tasks { get; init; } = [];
}
