namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class Assessment
{
    public static Assessment Create(
        long residentId,
        long assessedBy,
        int adlTotalScore,
        bool isOverridden,
        long suggestedCareLevelId,
        long confirmedCareLevelId,
        DateTimeOffset createdAt)
    {
        return new Assessment
        {
            ResidentId = residentId,
            AssessedBy = assessedBy,
            AdlTotalScore = adlTotalScore,
            IsOverridden = isOverridden,
            SuggestedCareLevelId = suggestedCareLevelId,
            ConfirmedCareLevelId = confirmedCareLevelId,
            CreatedAt = createdAt
        };
    }
    // not need yet
    // public void Update(
    //     int adlTotalScore,
    //     bool isOverridden,
    //     long suggestedCareLevelId,
    //     long confirmedCareLevelId)
    // {
    //     AdlTotalScore = adlTotalScore;
    //     IsOverridden = isOverridden;
    //     SuggestedCareLevelId = suggestedCareLevelId;
    //     ConfirmedCareLevelId = confirmedCareLevelId;
    // }
}