using System;
using System.Collections.Generic;

namespace NursingHome.Application.Features.CnaDashboard.DTOs;

public class CnaDashboardDto
{
    public string ShiftName { get; set; } = null!;
    public TimeOnly ShiftStart { get; set; }
    public TimeOnly ShiftEnd { get; set; }
    public int ShiftProgressPercentage { get; set; }
    public List<CnaResidentDto> AssignedResidents { get; set; } = new();
    public List<CnaAdlTaskDto> AdlTasks { get; set; } = new();
    public List<CnaTurnTimerDto> TurnTimers { get; set; } = new();
}

public class CnaResidentDto
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string RoomNumber { get; set; } = null!;
    public string BedNumber { get; set; } = null!;
    public string DietaryPreference { get; set; } = null!;
}

public class CnaAdlTaskDto
{
    public long Id { get; set; }
    public long ResidentId { get; set; }
    public string ResidentName { get; set; } = null!;
    public string TaskType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTimeOffset ScheduledTime { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
}

public class CnaTurnTimerDto
{
    public long Id { get; set; }
    public long ResidentId { get; set; }
    public string ResidentName { get; set; } = null!;
    public string RoomNumber { get; set; } = null!;
    public DateTimeOffset ScheduledTime { get; set; }
    public int RemainingMinutes { get; set; }
    public string Status { get; set; } = null!; // "Normal", "Warning", "Overdue"
}
