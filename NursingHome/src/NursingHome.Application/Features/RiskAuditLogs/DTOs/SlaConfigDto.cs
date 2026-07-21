namespace NursingHome.Application.Features.RiskAuditLogs.DTOs;

// SC_011 / AD-09 — SLA Configuration.
// ExternalReportRequired, ReportingDeadline text và RegulatoryBody được suy ra / hardcode
// trong handler vì bảng sla_configs chỉ có sla_window_hrs + severity_id (không có 2 cột này).
public record SlaConfigDto(
    long? Id,                    // sla_config id — null nếu severity chưa có cấu hình (không editable)
    long SeverityId,
    string SeverityName,
    int? SlaWindowHrs,
    bool ExternalReportRequired,
    string ReportingDeadline,    // "24 hours" / "— (not required)"
    string? RegulatoryBody,
    bool Editable
);
