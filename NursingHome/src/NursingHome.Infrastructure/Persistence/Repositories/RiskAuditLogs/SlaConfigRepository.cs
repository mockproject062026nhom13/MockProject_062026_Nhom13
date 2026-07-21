using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.RiskAuditLogs;

public class SlaConfigRepository(NursingHomeDbContext context) : ISlaConfigRepository
{
    // Hardcode theo plan: sla_configs không có cột regulatory_body / external_report_required.
    private const string RegulatoryBodyName = "CA Dept. of Public Health";

    private readonly NursingHomeDbContext _context = context;

    public async Task<List<SlaConfigDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Lấy toàn bộ severity rồi ghép cấu hình SLA (nếu có) — severity chưa có config vẫn hiển thị (Edit = N/A).
        var severities = await _context.Set<IncidentSeverity>()
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync(cancellationToken);

        var configBySeverity = await _context.Set<SlaConfig>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var configLookup = configBySeverity
            .GroupBy(c => c.SeverityId)
            .ToDictionary(g => g.Key, g => g.OrderBy(c => c.Id).First());

        return severities.Select(s =>
        {
            configLookup.TryGetValue(s.Id, out var config);

            var externalRequired = IsExternalReportRequired(s.LevelName);
            var windowHrs = config?.SlaWindowHrs;

            return new SlaConfigDto(
                Id: config?.Id,
                SeverityId: s.Id,
                SeverityName: s.LevelName,
                SlaWindowHrs: externalRequired ? windowHrs : null,
                ExternalReportRequired: externalRequired,
                ReportingDeadline: externalRequired && windowHrs.HasValue
                    ? $"{windowHrs} hours"
                    : "— (not required)",
                RegulatoryBody: externalRequired ? RegulatoryBodyName : null,
                Editable: externalRequired && config is not null
            );
        }).ToList();
    }

    public async Task<bool> UpdateWindowAsync(long id, int slaWindowHrs, CancellationToken cancellationToken = default)
    {
        var config = await _context.Set<SlaConfig>()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (config is null) return false;

        // SlaWindowHrs có setter private — set qua change tracker của EF (không đụng entity scaffold).
        _context.Entry(config).Property(c => c.SlaWindowHrs).CurrentValue = slaWindowHrs;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    // Tier thấp nhất (Low/Minor) không cần báo cáo ra ngoài; các tier còn lại báo cáo cơ quan quản lý bang.
    private static bool IsExternalReportRequired(string levelName)
    {
        var normalized = levelName.Trim().ToUpperInvariant();
        return normalized is not ("LOW" or "MINOR");
    }
}
