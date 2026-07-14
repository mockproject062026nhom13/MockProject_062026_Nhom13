using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class Incident
{
    public static Incident Create(
        string incidentType,
        string location,
        string description,
        string? witness,
        string immediateActionsTaken,
        DateTimeOffset slaDeadline,
        long residentId,
        long severityId,
        long reportedBy,
        DateTimeOffset reportedAt)
    {
        return new Incident
        {
            IncidentType = incidentType,
            Status = "OPEN",
            Location = location,
            Description = description,
            Witness = witness,
            ImmediateActionsTaken = immediateActionsTaken,
            SlaDeadline = slaDeadline,
            ResidentId = residentId,
            SeverityId = severityId,
            ReportedBy = reportedBy,
            ReportedAt = reportedAt,           
        };
    }
}