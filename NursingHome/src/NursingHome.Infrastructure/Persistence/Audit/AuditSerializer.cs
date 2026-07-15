using System.Text.Json;
using System.Text.Json.Serialization;

namespace NursingHome.Infrastructure.Persistence.Audit;

internal static class AuditSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    public static string? Serialize(IDictionary<string, object?> values)
    {
        if (values.Count == 0)
        {
            return null;
        }

        return JsonSerializer.Serialize(values, Options);
    }
}