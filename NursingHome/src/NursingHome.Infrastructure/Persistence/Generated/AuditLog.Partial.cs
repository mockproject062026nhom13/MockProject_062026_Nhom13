using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class AuditLog
{
    private AuditLog()
    {
    }

    public static AuditLog Create(
        string tableName,
        string recordId,
        string action,
        string? oldData,
        string? newData,
        long performedBy,
        DateTimeOffset performedAt,
        string? ipAddress = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordId);
        ArgumentException.ThrowIfNullOrWhiteSpace(action);

        return new AuditLog
        {
            TableName = tableName.Trim(),
            RecordId = recordId.Trim(),
            Action = action.Trim().ToUpperInvariant(),
            OldData = oldData,
            NewData = newData,
            PerformedBy = performedBy,
            PerformedAt = performedAt,
            IpAddress = ipAddress
        };
    }
}