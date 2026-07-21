namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class AssessmentDetail
{
    public static AssessmentDetail Create(
        long assessmentId,
        long metricId,
        int score,
        string? notes)
    {
        return new AssessmentDetail
        {
            AssessmentId = assessmentId,
            MetricId = metricId,
            Score = score,
            Notes = notes
        };
    }

    public void Update(
        long metricId,
        int score,
        string? notes)
    {
        MetricId = metricId;
        Score = score;
        Notes = notes;
    }
}