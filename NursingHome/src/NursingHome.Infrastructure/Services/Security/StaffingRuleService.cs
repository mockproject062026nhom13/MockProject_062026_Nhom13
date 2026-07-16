using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Application.Features.Facilities.DTOs.Facility;
using NursingHome.Domain.Constants;
using NursingHome.Domain.Exceptions;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Services.Security;

public class StaffingRuleService : IStaffingRuleService
{
    private readonly NursingHomeDbContext _dbContext;
    private readonly ILogger<StaffingRuleService> _logger;
    private readonly IConfiguration _configuration;

    private static readonly string StorageDirectory = Path.Combine(Path.GetTempPath(), "nursinghome");
    private static readonly string ConfigPath = Path.Combine(StorageDirectory, "staffing_ratios.json");
    private static readonly string LogPath = Path.Combine(StorageDirectory, "compliance_status_logs.json");

    private static readonly object FileLock = new();

    public StaffingRuleService(NursingHomeDbContext dbContext, ILogger<StaffingRuleService> logger, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;

        Directory.CreateDirectory(StorageDirectory);
    }

    public async Task<StaffingRuleConfigDto> GetRulesAsync(long facilityId, CancellationToken cancellationToken = default)
    {
        // 1. Kiểm tra Facility tồn tại
        var facilityExists = await _dbContext.Facilities.AnyAsync(f => f.Id == facilityId, cancellationToken);
        if (!facilityExists)
        {
            throw new NotFoundException("Facility", facilityId);
        }

        // 2. Load ratios từ file JSON
        var config = LoadRatiosFromFile();

        // 3. Đảm bảo cấu hình luôn đầy đủ cho tất cả LOC (CareLevel) trong DB
        var careLevels = await _dbContext.CareLevels
            .Where(c => !c.IsDeleted)
            .Select(c => c.LevelCode)
            .ToListAsync(cancellationToken);

        bool needsSave = false;

        foreach (var loc in careLevels)
        {
            if (!config.Standard.ContainsKey(loc))
            {
                config.Standard[loc] = new ShiftRatiosDto
                {
                    Day = new RatioDetailDto { Nurse = 15, Cna = 8 },
                    Evening = new RatioDetailDto { Nurse = 20, Cna = 10 },
                    Night = new RatioDetailDto { Nurse = 30, Cna = 15 }
                };
                needsSave = true;
            }

            if (!config.Emergency.ContainsKey(loc))
            {
                config.Emergency[loc] = new ShiftRatiosDto
                {
                    Day = new RatioDetailDto { Nurse = 10, Cna = 6 },
                    Evening = new RatioDetailDto { Nurse = 15, Cna = 8 },
                    Night = new RatioDetailDto { Nurse = 20, Cna = 10 }
                };
                needsSave = true;
            }
        }

        if (needsSave)
        {
            SaveRatiosToFile(config);
        }

        return config;
    }

    public async Task UpdateRulesAsync(long facilityId, StaffingRuleConfigDto config, long performedByUserId, CancellationToken cancellationToken = default)
    {
        // 1. Kiểm tra User Admin
        var adminUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == performedByUserId, cancellationToken);
        if (adminUser == null)
        {
            throw new DomainException("Admin user not found.");
        }

        // 2. Load giới hạn luật định từ Configuration (.env / appsettings.json)
        var limits = LoadLimitsFromConfig();

        // 3. Validate không vượt quá giới hạn luật định (Tỷ lệ max_residents_per_nurse/cna không được lớn hơn luật định)
        ValidateRatiosAgainstLimits(config.Standard, limits, "Standard");
        ValidateRatiosAgainstLimits(config.Emergency, limits, "Emergency");

        // 4. Lấy Old Data phục vụ Audit Log
        var oldConfig = await GetRulesAsync(facilityId, cancellationToken);

        // 5. Lưu vào file JSON
        // Giữ nguyên cờ IsEmergencyMode hiện tại
        config.IsEmergencyMode = oldConfig.IsEmergencyMode;
        SaveRatiosToFile(config);

        // 6. Lưu Audit Log vào DB
        var auditLog = new AuditLog(
            "staffing_ratios",
            facilityId.ToString(),
            "UPDATE",
            JsonSerializer.Serialize(oldConfig),
            JsonSerializer.Serialize(config),
            performedByUserId,
            DateTimeOffset.UtcNow,
            null
        );

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Staffing ratio rules updated successfully by Admin {AdminId}", performedByUserId);
    }

    public async Task<StaffingRuleConfigDto> ToggleEmergencyModeAsync(long facilityId, long performedByUserId, CancellationToken cancellationToken = default)
    {
        // 1. Kiểm tra User Admin
        var adminUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == performedByUserId, cancellationToken);
        if (adminUser == null)
        {
            throw new DomainException("Admin user not found.");
        }

        // 2. Toggle emergency mode
        var config = await GetRulesAsync(facilityId, cancellationToken);
        config.IsEmergencyMode = !config.IsEmergencyMode;
        SaveRatiosToFile(config);

        // 3. Lưu Audit Log vào DB
        var auditLog = new AuditLog(
            "staffing_ratios",
            facilityId.ToString(),
            "TOGGLE_EMERGENCY",
            JsonSerializer.Serialize(!config.IsEmergencyMode),
            JsonSerializer.Serialize(config.IsEmergencyMode),
            performedByUserId,
            DateTimeOffset.UtcNow,
            null
        );

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Emergency/Outbreak mode toggled to {Mode} by Admin {AdminId}", config.IsEmergencyMode, performedByUserId);

        return config;
    }

    public async Task<ComplianceResultDto> GetComplianceAsync(long facilityId, DateOnly date, CancellationToken cancellationToken = default)
    {
        var facilityExists = await _dbContext.Facilities.AnyAsync(f => f.Id == facilityId, cancellationToken);
        if (!facilityExists)
        {
            throw new NotFoundException("Facility", facilityId);
        }

        // 1. Đọc config active ratios
        var config = await GetRulesAsync(facilityId, cancellationToken);
        var activeRatios = config.IsEmergencyMode ? config.Emergency : config.Standard;

        // 2. Count Active Residents grouped by CareLevel (LOC)
        var residents = await _dbContext.Residents
            .AsNoTracking()
            .Where(r => r.Status == "Active" &&
                        r.Admissions.Any(a => a.FacilityId == facilityId && a.DischargeDate == null))
            .Select(r => new
            {
                r.Id,
                LocCode = r.ResidentCareLevelHistories
                    .Where(h => h.StartDate <= date && (h.EndDate == null || h.EndDate >= date))
                    .OrderByDescending(h => h.StartDate)
                    .Select(h => h.CareLevel.LevelCode)
                    .FirstOrDefault() ?? "LOC-1"
            })
            .ToListAsync(cancellationToken);

        var locCounts = residents.GroupBy(r => r.LocCode).ToDictionary(g => g.Key, g => g.Count());
        int totalCensus = residents.Count;

        // 3. Lấy danh sách ca (Shifts)
        var shifts = await _dbContext.Shifts
            .AsNoTracking()
            .Where(s => s.FacilityId == facilityId)
            .ToListAsync(cancellationToken);

        // 4. Lấy danh sách Shift Assignments
        var assignments = await _dbContext.ShiftAssignments
            .Include(sa => sa.User).ThenInclude(u => u.Role)
            .AsNoTracking()
            .Where(sa => sa.Shift.FacilityId == facilityId && sa.WorkDate == date && sa.Status == "SCHEDULED")
            .ToListAsync(cancellationToken);

        var shiftDetails = new List<ShiftComplianceDetailDto>();

        foreach (var shift in shifts)
        {
            var shiftNameUpper = shift.ShiftName.ToUpper();
            string shiftType = "Day";
            if (shiftNameUpper.Contains("EVENING") || shiftNameUpper.Contains("AFTERNOON"))
            {
                shiftType = "Evening";
            }
            else if (shiftNameUpper.Contains("NIGHT"))
            {
                shiftType = "Night";
            }

            // Tính số lượng nhân sự yêu cầu
            decimal requiredNursesDecimal = 0;
            decimal requiredCnasDecimal = 0;

            foreach (var loc in locCounts)
            {
                int nurseRatio = 15;
                int cnaRatio = 8;

                if (activeRatios.TryGetValue(loc.Key, out var locRatio))
                {
                    var shiftRatio = shiftType switch
                    {
                        "Day" => locRatio.Day,
                        "Evening" => locRatio.Evening,
                        "Night" => locRatio.Night,
                        _ => locRatio.Day
                    };
                    nurseRatio = shiftRatio.Nurse > 0 ? shiftRatio.Nurse : 15;
                    cnaRatio = shiftRatio.Cna > 0 ? shiftRatio.Cna : 8;
                }

                requiredNursesDecimal += (decimal)loc.Value / nurseRatio;
                requiredCnasDecimal += (decimal)loc.Value / cnaRatio;
            }

            int requiredNurses = (int)Math.Ceiling(requiredNursesDecimal);
            int requiredCnas = (int)Math.Ceiling(requiredCnasDecimal);

            // Đếm số lượng nhân sự thực tế đã gán
            var shiftAssignments = assignments.Where(sa => sa.ShiftId == shift.Id).ToList();
            int scheduledNurses = 0;
            int scheduledCnas = 0;

            foreach (var sa in shiftAssignments)
            {
                var roleName = sa.User.Role.RoleName;
                var isNurse = string.Equals(roleName, RoleConstants.NurseLpnRn, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(roleName, RoleConstants.DonDirectorOfNursing, StringComparison.OrdinalIgnoreCase);
                var isCna = string.Equals(roleName, RoleConstants.CnaCaregiver, StringComparison.OrdinalIgnoreCase);

                if (isNurse) scheduledNurses++;
                else if (isCna) scheduledCnas++;
            }

            // Tính toán tuân thủ
            int totalRequired = requiredNurses + requiredCnas;
            int totalScheduled = scheduledNurses + scheduledCnas;
            decimal compliancePercentage = 100;
            if (totalRequired > 0)
            {
                compliancePercentage = Math.Round(((decimal)totalScheduled / totalRequired) * 100, 2);
            }

            string status = "Compliant";
            if (totalScheduled == 0)
            {
                status = "Unscheduled";
                compliancePercentage = 0;
            }
            else if (compliancePercentage < 70)
            {
                status = "Critical";
            }
            else if (compliancePercentage < 85)
            {
                status = "Warning";
            }

            int missingNurses = Math.Max(0, requiredNurses - scheduledNurses);
            int missingCnas = Math.Max(0, requiredCnas - scheduledCnas);

            string alertTooltip = status switch
            {
                "Unscheduled" => "No staff scheduled yet.",
                "Compliant" => "Staffing ratio compliant.",
                _ => $"Missing {missingNurses} Nurse(s), {missingCnas} CNA(s)."
            };

            shiftDetails.Add(new ShiftComplianceDetailDto
            {
                ShiftId = shift.Id,
                ShiftName = shift.ShiftName,
                ShiftTime = $"{shift.StartTime:hh\\:mm} - {shift.EndTime:hh\\:mm}",
                RequiredNurses = requiredNurses,
                RequiredCnas = requiredCnas,
                ScheduledNurses = scheduledNurses,
                ScheduledCnas = scheduledCnas,
                MissingNurses = missingNurses,
                MissingCnas = missingCnas,
                CompliancePercentage = compliancePercentage,
                Status = status,
                AlertTooltip = alertTooltip
            });
        }

        var result = new ComplianceResultDto
        {
            Census = totalCensus,
            Date = date,
            IsEmergencyModeActive = config.IsEmergencyMode,
            ShiftDetails = shiftDetails
        };

        // 5. Lưu kết quả vào Compliance Logs JSON
        AppendToComplianceLogs(facilityId, date, result);

        return result;
    }

    #region Helper Methods

    private StaffingRuleConfigDto LoadRatiosFromFile()
    {
        lock (FileLock)
        {
            if (!File.Exists(ConfigPath))
            {
                return new StaffingRuleConfigDto();
            }

            try
            {
                var content = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<StaffingRuleConfigDto>(content) ?? new StaffingRuleConfigDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load staffing ratios from JSON file. Using empty default.");
                return new StaffingRuleConfigDto();
            }
        }
    }

    private void SaveRatiosToFile(StaffingRuleConfigDto config)
    {
        lock (FileLock)
        {
            try
            {
                var content = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write staffing ratios to JSON file.");
                throw new DomainException("Could not persist configuration changes.");
            }
        }
    }

    private Dictionary<string, RatioDetailDto> LoadLimitsFromConfig()
    {
        var limits = new Dictionary<string, RatioDetailDto>();

        int dayNurse = int.Parse(_configuration["FacilityLimits:Day:Nurse"] ?? throw new InvalidOperationException("FacilityLimits:Day:Nurse is missing in configuration."));
        int dayCna = int.Parse(_configuration["FacilityLimits:Day:Cna"] ?? throw new InvalidOperationException("FacilityLimits:Day:Cna is missing in configuration."));
        int eveningNurse = int.Parse(_configuration["FacilityLimits:Evening:Nurse"] ?? throw new InvalidOperationException("FacilityLimits:Evening:Nurse is missing in configuration."));
        int eveningCna = int.Parse(_configuration["FacilityLimits:Evening:Cna"] ?? throw new InvalidOperationException("FacilityLimits:Evening:Cna is missing in configuration."));
        int nightNurse = int.Parse(_configuration["FacilityLimits:Night:Nurse"] ?? throw new InvalidOperationException("FacilityLimits:Night:Nurse is missing in configuration."));
        int nightCna = int.Parse(_configuration["FacilityLimits:Night:Cna"] ?? throw new InvalidOperationException("FacilityLimits:Night:Cna is missing in configuration."));

        limits["Day"] = new RatioDetailDto { Nurse = dayNurse, Cna = dayCna };
        limits["Evening"] = new RatioDetailDto { Nurse = eveningNurse, Cna = eveningCna };
        limits["Night"] = new RatioDetailDto { Nurse = nightNurse, Cna = nightCna };

        return limits;
    }

    private void ValidateRatiosAgainstLimits(Dictionary<string, ShiftRatiosDto> ratios, Dictionary<string, RatioDetailDto> limits, string modeName)
    {
        foreach (var loc in ratios)
        {
            var shiftRatio = loc.Value;

            // Kiểm tra ca Ngày (Day)
            if (limits.TryGetValue("Day", out var dayLimit))
            {
                if (shiftRatio.Day.Nurse > dayLimit.Nurse)
                    throw new DomainException($"[{modeName} - {loc.Key}] Day Nurse ratio ({shiftRatio.Day.Nurse}) exceeds legal limit ({dayLimit.Nurse} residents per staff).");
                if (shiftRatio.Day.Cna > dayLimit.Cna)
                    throw new DomainException($"[{modeName} - {loc.Key}] Day CNA ratio ({shiftRatio.Day.Cna}) exceeds legal limit ({dayLimit.Cna} residents per staff).");
            }

            // Kiểm tra ca Chiều (Evening)
            if (limits.TryGetValue("Evening", out var eveningLimit))
            {
                if (shiftRatio.Evening.Nurse > eveningLimit.Nurse)
                    throw new DomainException($"[{modeName} - {loc.Key}] Evening Nurse ratio ({shiftRatio.Evening.Nurse}) exceeds legal limit ({eveningLimit.Nurse} residents per staff).");
                if (shiftRatio.Evening.Cna > eveningLimit.Cna)
                    throw new DomainException($"[{modeName} - {loc.Key}] Evening CNA ratio ({shiftRatio.Evening.Cna}) exceeds legal limit ({eveningLimit.Cna} residents per staff).");
            }

            // Kiểm tra ca Đêm (Night)
            if (limits.TryGetValue("Night", out var nightLimit))
            {
                if (shiftRatio.Night.Nurse > nightLimit.Nurse)
                    throw new DomainException($"[{modeName} - {loc.Key}] Night Nurse ratio ({shiftRatio.Night.Nurse}) exceeds legal limit ({nightLimit.Nurse} residents per staff).");
                if (shiftRatio.Night.Cna > nightLimit.Cna)
                    throw new DomainException($"[{modeName} - {loc.Key}] Night CNA ratio ({shiftRatio.Night.Cna}) exceeds legal limit ({nightLimit.Cna} residents per staff).");
            }
        }
    }

    private void AppendToComplianceLogs(long facilityId, DateOnly date, ComplianceResultDto result)
    {
        lock (FileLock)
        {
            try
            {
                List<ComplianceResultDto> logs = new();
                if (File.Exists(LogPath))
                {
                    var content = File.ReadAllText(LogPath);
                    logs = JsonSerializer.Deserialize<List<ComplianceResultDto>>(content) ?? new List<ComplianceResultDto>();
                }

                // Loại bỏ log cũ của cùng ngày và facility để update bản mới nhất
                logs.RemoveAll(l => l.Date == date);
                logs.Add(result);

                var newContent = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(LogPath, newContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log compliance status to JSON history.");
            }
        }
    }

    #endregion
}
