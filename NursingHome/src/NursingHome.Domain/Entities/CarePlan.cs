// namespace NursingHome.Domain.CarePlans;

// public class CarePlan
// {
//     private readonly List<CareGoal> _careGoals = [];
//     private readonly List<CareIntervention> _careInterventions = [];

//     private CarePlan()
//     {
//         // Required by EF
//     }

//     public CarePlan(
//         long residentId,
//         string status,
//         bool significantChangeFlag)
//     {
//         ResidentId = residentId;
//         Status = status;
//         SignificantChangeFlag = significantChangeFlag;

//         CreatedAt = DateTimeOffset.UtcNow;
//         UpdatedAt = DateTimeOffset.UtcNow;
//     }

//     public long Id { get; private set; }

//     public long ResidentId { get; private set; }

//     public string Status { get; private set; }

//     public bool SignificantChangeFlag { get; private set; }

//     public bool IsDeleted { get; private set; }

//     public DateTimeOffset CreatedAt { get; private set; }

//     public DateTimeOffset UpdatedAt { get; private set; }

//     public Resident Resident { get; private set; } = null!;

//     public IReadOnlyCollection<CareGoal> CareGoals => _careGoals;

//     public IReadOnlyCollection<CareIntervention> CareInterventions => _careInterventions;

//     public void UpdateStatus(string status)
//     {
//         Status = status;
//         UpdatedAt = DateTimeOffset.UtcNow;
//     }

//     public void MarkSignificantChange()
//     {
//         SignificantChangeFlag = true;
//         UpdatedAt = DateTimeOffset.UtcNow;
//     }

//     public void AddGoal(CareGoal goal)
//     {
//         _careGoals.Add(goal);
//         UpdatedAt = DateTimeOffset.UtcNow;
//     }

//     public void AddIntervention(CareIntervention intervention)
//     {
//         _careInterventions.Add(intervention);
//         UpdatedAt = DateTimeOffset.UtcNow;
//     }

//     public void SoftDelete()
//     {
//         IsDeleted = true;
//         UpdatedAt = DateTimeOffset.UtcNow;
//     }
// }