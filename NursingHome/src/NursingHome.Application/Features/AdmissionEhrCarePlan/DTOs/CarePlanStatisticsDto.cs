namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public record CarePlanStatisticDto
{
    public int TotalPlans{get;init;}
    public int DraftCount{get;init;}
    public int PendingReviewCount{get;init;}
    public int ReviewDueCount{get;init;} 
}