using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.RiskAuditLogs.Commands;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.RiskAuditLogs;

public class IncidentRepository(NursingHomeDbContext _context) : IIncidentRepository
{
    public async Task<long> CreateIncidentAsync(CreateIncidentCommand command, long currentUserId, CancellationToken cancellationToken)
    {
        // Check foreign key again (error 500)
        var residentExists = await _context.Residents.AnyAsync(r => r.Id == command.ResidentId, cancellationToken);
        var severityExists = await _context.IncidentSeverities.AnyAsync(s => s.Id == command.SeverityId, cancellationToken);
        var userExists = await _context.Users.AnyAsync(u => u.Id == currentUserId, cancellationToken);

        if (!residentExists || !severityExists || !userExists) return 0;

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Tính SLA Deadline
            var slaConfig = await _context.SlaConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SeverityId == command.SeverityId, cancellationToken);
                
            int slaWindowHrs = slaConfig?.SlaWindowHrs ?? 24; 
            var slaDeadline = command.ReportedAt.AddHours(slaWindowHrs); 

            // Khởi tạo incident
            var incident = Incident.Create(
                incidentType: command.IncidentType,
                location: command.Location,
                description: command.Description,
                witness: command.Witness,
                immediateActionsTaken: command.ImmediateActionsTaken,
                slaDeadline: slaDeadline,
                residentId: command.ResidentId,
                severityId: command.SeverityId,
                reportedBy: currentUserId,
                reportedAt: command.ReportedAt
            );
            
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync(cancellationToken); 

            var timeline = IncidentTimeline.Create(
                incidentId: incident.Id, 
                action: "REPORTED",
                reason: "Initial incident report submitted.",
                actor: currentUserId,
                createdAt: DateTimeOffset.UtcNow
            );
            
            _context.IncidentTimelines.Add(timeline);
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return incident.Id;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return 0;
        }
    }
}