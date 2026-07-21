using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

// SC_027 — factory + hành vi cho CarePlan (đặt riêng, không đụng scaffold generated).
public partial class CarePlan
{
    public static CarePlan CreateDraft(long residentId, bool significantChangeFlag)
    {
        var now = DateTimeOffset.UtcNow;
        return new CarePlan
        {
            Status = "DRAFT",
            ResidentId = residentId,
            SignificantChangeFlag = significantChangeFlag,
            IsDeleted = false,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public void Activate()
    {
        Status = "ACTIVE";
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
