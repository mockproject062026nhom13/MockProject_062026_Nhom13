namespace NursingHome.Infrastructure.Persistence.Audit;

public static class AuditConstants
{
    public const string Create = "INSERT";
    public const string Update = "UPDATE";
    public const string Delete = "DELETE";

    /// <summary>
    /// Entities không cần audit để tránh recursive hoặc dữ liệu hệ thống.
    /// </summary>
    public static readonly HashSet<Type> IgnoredEntities =
    [
        typeof(Generated.AuditLog)
    ];

    /// <summary>
    /// Các property không cần ghi nhận thay đổi.
    /// Có thể bổ sung sau nếu cần.
    /// </summary>
    public static readonly HashSet<string> IgnoredProperties =
    [
        "UpdatedAt"
        // "RowVersion",
        // "ConcurrencyStamp"
    ];
}