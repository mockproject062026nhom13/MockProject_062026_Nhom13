using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class user_facility
{
    protected user_facility()
    {
        
    }
    public user_facility(Guid userId, Guid facilityId)
    {
        this.user_id = userId;
        this.facility_id = facilityId;
    }
}