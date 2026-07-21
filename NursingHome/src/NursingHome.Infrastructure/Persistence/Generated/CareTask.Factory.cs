using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class CareTask
{
    public static CareTask CreatePending(string taskType, DateTimeOffset scheduledTime, long? assignedCnaId = null)
        => new CareTask
        {
            TaskType = taskType,
            Status = "PENDING",
            IsAbnormalFlagged = false,
            ScheduledTime = scheduledTime,
            AssignedCnaId = assignedCnaId
        };
}
