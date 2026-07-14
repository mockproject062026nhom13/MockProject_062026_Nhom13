using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class CareTask
{
    public void Complete(DateTimeOffset completedAt)
    {
        Status = "Completed";
        CompletedAt = completedAt;
    }
}
