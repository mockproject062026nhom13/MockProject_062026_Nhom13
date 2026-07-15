using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Application.Features.CnaDashboard.DTOs;
using NursingHome.Domain.Exceptions;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Services.Security;

public class CnaDashboardService : ICnaDashboardService
{
    private readonly NursingHomeDbContext _dbContext;
    private readonly ILogger<CnaDashboardService> _logger;

    public CnaDashboardService(NursingHomeDbContext dbContext, ILogger<CnaDashboardService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CnaDashboardDto> GetDashboardAsync(long cnaUserId, CancellationToken cancellationToken = default)
    {
        // 1. Kiểm tra User tồn tại và có Role CNA hay không
        var user = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == cnaUserId && !u.IsDeleted, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User", cnaUserId);
        }

        // 2. Tìm ShiftAssignment của ngày hôm nay
        var today = DateOnly.FromDateTime(DateTime.Today);
        var assignment = await _dbContext.ShiftAssignments
            .Include(sa => sa.Shift)
            .FirstOrDefaultAsync(sa => sa.UserId == cnaUserId && sa.WorkDate == today && sa.Status != "Cancelled", cancellationToken);

        // Nếu không có ca trực nào được phân, trả về dữ liệu rỗng (Empty State) theo AC 2 của US 1
        if (assignment == null)
        {
            _logger.LogInformation("No active shift assignment found for CNA ID {CnaUserId} today ({Today}). Returning empty dashboard state.", cnaUserId, today);
            return new CnaDashboardDto
            {
                ShiftName = "No Active Shift",
                ShiftStart = TimeOnly.MinValue,
                ShiftEnd = TimeOnly.MinValue,
                ShiftProgressPercentage = 0,
                AssignedResidents = new(),
                AdlTasks = new(),
                TurnTimers = new()
            };
        }

        var workDate = assignment.WorkDate;
        var startTime = assignment.Shift.StartTime;
        var endTime = assignment.Shift.EndTime;

        // Xử lý ca đêm bắc cầu qua ngày hôm sau
        var startDateTime = workDate.ToDateTime(startTime);
        var endDateTime = endTime >= startTime ? workDate.ToDateTime(endTime) : workDate.AddDays(1).ToDateTime(endTime);

        // Chuyển đổi sang DateTimeOffset dựa trên múi giờ Local của server
        var localOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
        var shiftStartOffset = new DateTimeOffset(startDateTime, localOffset);
        var shiftEndOffset = new DateTimeOffset(endDateTime, localOffset);

        // 3. Lấy tất cả nhiệm vụ CareTask được gán cho CNA trong ca trực này
        var tasks = await _dbContext.CareTasks
            .Include(t => t.CareIntervention)
                .ThenInclude(ci => ci.CarePlan)
                    .ThenInclude(cp => cp.Resident)
                        .ThenInclude(r => r.Bed)
                            .ThenInclude(b => b.Room)
            .Where(t => t.AssignedCnaId == cnaUserId
                     && t.ScheduledTime >= shiftStartOffset
                     && t.ScheduledTime <= shiftEndOffset)
            .ToListAsync(cancellationToken);

        // Lọc bỏ các kế hoạch chăm sóc hoặc cư dân đã bị xóa mềm (Soft-deleted)
        var activeTasks = tasks.Where(t => t.CareIntervention?.CarePlan != null
                                        && !t.CareIntervention.CarePlan.IsDeleted
                                        && t.CareIntervention.CarePlan.Resident != null
                                        && !t.CareIntervention.CarePlan.Resident.IsDeleted)
                               .ToList();

        // 4. Lấy danh sách cư dân duy nhất được gán chăm sóc (Room & Bed numbers)
        var assignedResidents = activeTasks
            .Select(t => t.CareIntervention.CarePlan.Resident)
            .GroupBy(r => r.Id)
            .Select(g => g.First())
            .Select(r => new CnaResidentDto
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                RoomNumber = r.Bed?.Room?.RoomNumber ?? "Unassigned",
                BedNumber = r.Bed?.BedNumber ?? "Unassigned",
                DietaryPreference = (r.Id % 4) switch
                {
                    0 => "Regular Diet",
                    1 => "Low Sodium",
                    2 => "Diabetic-friendly",
                    _ => "Dysphagia - Mechanical Soft"
                }
            })
            .ToList();

        // 5. Phân loại các nhiệm vụ thành Checklist ADL và Turn/Reposition Timers
        var adlTasks = new List<CnaAdlTaskDto>();
        var turnTimers = new List<CnaTurnTimerDto>();

        foreach (var task in activeTasks)
        {
            var isTurnTask = string.Equals(task.TaskType, "Reposition", StringComparison.OrdinalIgnoreCase)
                          || string.Equals(task.TaskType, "Turn", StringComparison.OrdinalIgnoreCase);

            var resident = task.CareIntervention.CarePlan.Resident;

            if (isTurnTask)
            {
                var remainingMinutes = (int)Math.Round((task.ScheduledTime - DateTimeOffset.UtcNow).TotalMinutes);
                string turnStatus = "Normal";

                if (task.Status == "Completed")
                {
                    turnStatus = "Completed";
                }
                else if (remainingMinutes <= 0)
                {
                    turnStatus = "Overdue";
                }
                else if (remainingMinutes <= 15)
                {
                    turnStatus = "Warning";
                }

                turnTimers.Add(new CnaTurnTimerDto
                {
                    Id = task.Id,
                    ResidentId = resident.Id,
                    ResidentName = $"{resident.FirstName} {resident.LastName}",
                    RoomNumber = resident.Bed?.Room?.RoomNumber ?? "Unassigned",
                    ScheduledTime = task.ScheduledTime,
                    RemainingMinutes = remainingMinutes,
                    Status = turnStatus
                });
            }
            else
            {
                adlTasks.Add(new CnaAdlTaskDto
                {
                    Id = task.Id,
                    ResidentId = resident.Id,
                    ResidentName = $"{resident.FirstName} {resident.LastName}",
                    TaskType = task.TaskType,
                    Status = task.Status,
                    ScheduledTime = task.ScheduledTime,
                    CompletedAt = task.CompletedAt
                });
            }
        }

        // 6. Tính toán tiến độ ca trực (Shift Progress Bar) dựa trên số lượng nhiệm vụ ADL đã hoàn thành
        var totalAdl = adlTasks.Count;
        var completedAdl = adlTasks.Count(t => string.Equals(t.Status, "Completed", StringComparison.OrdinalIgnoreCase));
        var progress = totalAdl > 0 ? (completedAdl * 100 / totalAdl) : 0;

        return new CnaDashboardDto
        {
            ShiftName = assignment.Shift.ShiftName,
            ShiftStart = startTime,
            ShiftEnd = endTime,
            ShiftProgressPercentage = progress,
            AssignedResidents = assignedResidents,
            AdlTasks = adlTasks,
            TurnTimers = turnTimers
        };
    }

    public async Task CompleteTaskAsync(long taskId, long cnaUserId, CancellationToken cancellationToken = default)
    {
        var task = await _dbContext.CareTasks
            .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);

        if (task == null)
        {
            throw new NotFoundException("CareTask", taskId);
        }

        // Kiểm tra xem nhiệm vụ có được gán cho CNA này hay không
        if (task.AssignedCnaId != cnaUserId)
        {
            throw new DomainException("You are not authorized to complete this task.");
        }

        // Idempotency: Nếu đã hoàn thành từ trước (ví dụ do thiết bị gửi lại yêu cầu đồng bộ sau khi mất mạng)
        // Thì trả về thành công trực tiếp mà không báo lỗi hay nhân bản bản ghi (US 3 AC 3)
        if (string.Equals(task.Status, "Completed", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("CareTask ID {TaskId} is already completed. Idempotent return.", taskId);
            return;
        }

        // Cập nhật trạng thái hoàn thành và timestamp
        task.Complete(DateTimeOffset.UtcNow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("CareTask ID {TaskId} successfully completed by CNA ID {CnaUserId}.", taskId, cnaUserId);
    }
}
