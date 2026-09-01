using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class StaffingConfig
{
    // Constructor cho việc tạo mới Entity thông qua EF Core
    public StaffingConfig(long facilityId, decimal minHrsPerResidentDay, int warnBelowPercentage)
    {
        FacilityId = facilityId;
        MinHrsPerResidentDay = minHrsPerResidentDay;
        WarnBelowPercentage = warnBelowPercentage;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    // Method để update các thuộc tính private set
    public void UpdateConfig(decimal minHrsPerResidentDay, int warnBelowPercentage)
    {
        MinHrsPerResidentDay = minHrsPerResidentDay;
        WarnBelowPercentage = warnBelowPercentage;
    }
}
