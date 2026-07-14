using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NursingHome.Infrastructure.Persistence.Audit;

internal sealed class AuditEntry
{
    public EntityEntry Entry { get; }
    public long? PerformedBy { get; set; }

    public string? IpAddress { get; set; }
    public string TableName { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string RecordId { get; set; } = string.Empty;

    public Dictionary<string, object?> OldValues { get; } = [];

    public Dictionary<string, object?> NewValues { get; } = [];

    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }
}