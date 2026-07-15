using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NursingHome.Infrastructure.Persistence.Audit;

internal static class AuditHelper
{
    public static bool ShouldAudit(EntityEntry entry)
    {
        if (entry.State is EntityState.Detached or EntityState.Unchanged)
        {
            return false;
        }

        if (AuditConstants.IgnoredEntities.Contains(entry.Entity.GetType()))
        {
            return false;
        }

        return entry.State is EntityState.Added
            or EntityState.Modified
            or EntityState.Deleted;
    }

    public static AuditEntry CreateAuditEntry(EntityEntry entry)
    {
        var auditEntry = new AuditEntry(entry)
        {
            TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
            Action = MapAction(entry.State)
        };

        foreach (var property in entry.Properties)
        {
            if (property.Metadata.IsPrimaryKey())
            {
                auditEntry.RecordId = property.CurrentValue?.ToString() ?? string.Empty;
                continue;
            }

            if (AuditConstants.IgnoredProperties.Contains(property.Metadata.Name))
            {
                continue;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                    break;

                case EntityState.Deleted:
                    auditEntry.OldValues[property.Metadata.Name] = property.OriginalValue;
                    break;

                case EntityState.Modified:

                    if (!property.IsModified)
                    {
                        continue;
                    }

                    if (Equals(property.OriginalValue, property.CurrentValue))
                    {
                        continue;
                    }

                    auditEntry.OldValues[property.Metadata.Name] = property.OriginalValue;
                    auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                    break;
            }
        }

        return auditEntry;
    }

    private static string MapAction(EntityState state)
    {
        return state switch
        {
            EntityState.Added => AuditConstants.Create,
            EntityState.Modified => AuditConstants.Update,
            EntityState.Deleted => AuditConstants.Delete,
            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };
    }
}