namespace NursingHome.Application.Features.RiskAuditLogs.DTOs;

public enum ChartUnlockOperationStatus
{
    Success,
    NotFound,
    AlreadyUnlocked
}

public sealed class ChartUnlockOperationResult
{
    public ChartUnlockOperationStatus Status { get; init; }

    public UnlockResidentChartResultDto? Data { get; init; }

    public static ChartUnlockOperationResult Success(UnlockResidentChartResultDto data)
    {
        return new ChartUnlockOperationResult
        {
            Status = ChartUnlockOperationStatus.Success,
            Data = data
        };
    }

    public static ChartUnlockOperationResult NotFound()
    {
        return new ChartUnlockOperationResult
        {
            Status = ChartUnlockOperationStatus.NotFound
        };
    }

    public static ChartUnlockOperationResult AlreadyUnlocked()
    {
        return new ChartUnlockOperationResult
        {
            Status = ChartUnlockOperationStatus.AlreadyUnlocked
        };
    }
}
