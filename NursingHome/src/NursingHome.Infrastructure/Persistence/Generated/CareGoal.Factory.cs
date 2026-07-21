namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class CareGoal
{
    public static CareGoal Create(string status = "IN_PROGRESS")
        => new CareGoal { Status = status };
}
