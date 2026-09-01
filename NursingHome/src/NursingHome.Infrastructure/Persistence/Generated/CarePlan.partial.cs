namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class CarePlan
{
    private static readonly HashSet<string> AllowedStatuses =
    [
        "DISCONTINUED",
        "RESOLVED",
        "ACTIVE",
        "DRAFT"
    ];

    public void UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException(
                "Status is required.",
                nameof(status));
        }

        status = status.ToUpperInvariant();

        if (!AllowedStatuses.Contains(status))
        {
            throw new ArgumentException(
                $"Unsupported care plan status: {status}",
                nameof(status));
        }

        Status = status;
    }
}