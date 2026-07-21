using MediatR;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

// RegulatoryBody nhận vào để khớp form (màn hình cho sửa), nhưng KHÔNG lưu được vì
// sla_configs không có cột này — chỉ SlaWindowHrs (Deadline) được persist.
public record UpdateSlaConfigCommand(long Id, int SlaWindowHrs, string? RegulatoryBody) : IRequest<bool>;
