using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NursingHome.Application.Features.Facilities.Queries.GetStaffingCompliance;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Jobs;

public class StaffingComplianceBackgroundJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StaffingComplianceBackgroundJob> _logger;

    public StaffingComplianceBackgroundJob(IServiceProvider serviceProvider, ILogger<StaffingComplianceBackgroundJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StaffingComplianceBackgroundJob is starting.");

        // Chạy ngay lần đầu tiên khi ứng dụng vừa khởi động để test dễ dàng
        try
        {
            await CheckComplianceAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during initial staffing compliance check.");
        }

        using var timer = new PeriodicTimer(TimeSpan.FromHours(24)); // Các lần sau cách nhau 24h
        
        // Hoặc cấu hình chạy vào đúng 00:30 mỗi ngày
        // Ở mức Basic, chúng ta giả lập chạy mỗi ngày 1 lần bằng PeriodicTimer cho đơn giản

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await CheckComplianceAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing StaffingComplianceBackgroundJob.");
            }
        }
    }

    private async Task CheckComplianceAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking staffing compliance for all facilities...");

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NursingHomeDbContext>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        // 1. Lấy tất cả Facility
        var facilities = await dbContext.Facilities
            .Where(f => !f.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var facility in facilities)
        {
            // 2. Tính toán Compliance
            var query = new GetStaffingComplianceQuery(facility.Id, null);
            var complianceResult = await sender.Send(query, cancellationToken);

            if (!complianceResult.IsCompliant)
            {
                _logger.LogWarning($"Facility {facility.FacilityCode} is NON-COMPLIANT. Required: {complianceResult.RequiredHours}, Scheduled: {complianceResult.ScheduledHours}. Sending alerts...");

                // 3. Tìm tất cả DON của Facility này
                var donUsers = await dbContext.UserFacilities
                    .Include(uf => uf.User)
                    .ThenInclude(u => u.Role)
                    .Where(uf => uf.FacilityId == facility.Id && uf.User.Role.RoleName.StartsWith("DON") && !uf.User.IsDeleted)
                    .Select(uf => uf.UserId)
                    .ToListAsync(cancellationToken);

                // 4. Tạo Notification
                foreach (var userId in donUsers)
                {
                    var notification = new Notification(
                        title: "Staffing Shortage Detected",
                        type: "ALERT",
                        userId: userId
                    );
                    dbContext.Notifications.Add(notification);
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Staffing compliance check completed.");
    }
}
