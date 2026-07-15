using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Audit;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    private readonly List<AuditEntry> _auditEntries = [];

    private bool _isSavingAuditLog;

    public AuditSaveChangesInterceptor(
        ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (_isSavingAuditLog)
        {
            return result;
        }

        if (_auditEntries.Count == 0)
        {
            return result;
        }

        var context = eventData.Context;

        if (context is null)
        {
            return result;
        }

        await SaveAuditLogsAsync(context, cancellationToken);

        _auditEntries.Clear();

        return result;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        BuildAuditEntries(eventData.Context);

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        BuildAuditEntries(eventData.Context);

        return ValueTask.FromResult(result);
    }

    private void BuildAuditEntries(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        if (_isSavingAuditLog)
        {
            return;
        }

        _auditEntries.Clear();

        context.ChangeTracker.DetectChanges();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (!AuditHelper.ShouldAudit(entry))
            {
                continue;
            }

            var auditEntry = AuditHelper.CreateAuditEntry(entry);

            auditEntry.PerformedBy = _currentUserService.UserId;
            auditEntry.IpAddress = _currentUserService.IpAddress;

            _auditEntries.Add(auditEntry);
        }
    }

    private async Task SaveAuditLogsAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        _isSavingAuditLog = true;

        try
        {
            foreach (var auditEntry in _auditEntries)
            {
                UpdateRecordId(auditEntry);

                context.Set<AuditLog>().Add(
                    ConvertToAuditLog(auditEntry));
            }

            await context.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            _isSavingAuditLog = false;
        }
    }

    private static void UpdateRecordId(AuditEntry auditEntry)
    {
        if (!string.IsNullOrWhiteSpace(auditEntry.RecordId))
        {
            return;
        }

        var key = auditEntry.Entry.Properties
            .FirstOrDefault(p => p.Metadata.IsPrimaryKey());

        auditEntry.RecordId =
            key?.CurrentValue?.ToString() ?? string.Empty;
    }

    private static AuditLog ConvertToAuditLog(
        AuditEntry auditEntry)
    {
        return AuditLog.Create(
            tableName: auditEntry.TableName,
            recordId: auditEntry.RecordId,
            action: auditEntry.Action,
            oldData: AuditSerializer.Serialize(auditEntry.OldValues),
            newData: AuditSerializer.Serialize(auditEntry.NewValues),
            performedBy: auditEntry.PerformedBy ?? 0,
            DateTimeOffset.UtcNow,
            ipAddress: auditEntry.IpAddress);
    }
}