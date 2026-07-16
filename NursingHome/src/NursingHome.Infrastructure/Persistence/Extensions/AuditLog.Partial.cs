using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class AuditLog
{
    // Cần có parameterless constructor cho EF Core
    public AuditLog()
    {
    }

    public AuditLog(
        string tableName, 
        string recordId, 
        string action, 
        string? oldData, 
        string? newData, 
        long performedBy, 
        DateTimeOffset performedAt, 
        string? ipAddress)
    {
        TableName = tableName;
        RecordId = recordId;
        Action = action;
        OldData = oldData;
        NewData = newData;
        PerformedBy = performedBy;
        PerformedAt = performedAt;
        IpAddress = ipAddress;
    }
}
