using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class UserFacility
{
    protected UserFacility()
    {
        
    }
    public UserFacility(long userId, long facilityId)
    {
        this.UserId = userId;
        this.FacilityId = facilityId;
    }
}