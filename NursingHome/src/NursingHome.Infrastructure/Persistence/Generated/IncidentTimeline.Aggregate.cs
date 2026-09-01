using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class IncidentTimeline
{
    public static IncidentTimeline Create(
        long incidentId, 
        string action, 
        string reason, 
        long actor, 
        DateTimeOffset createdAt)
    {
        return new IncidentTimeline
        {
            IncidentId = incidentId,
            Action = action,
            Reason = reason,
            Actor = actor,
            CreatedAt = createdAt
        };
    }
}