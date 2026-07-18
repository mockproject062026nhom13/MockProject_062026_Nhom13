namespace NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

public record CarePlanDto
{
    public long Id{get; set;}
    public string ResidentName{get; set;} = string.Empty;
    public string RoomNumber{get; set;} = string.Empty;
    public string LocTier{get; set;} = string.Empty;
    public string Status{get; set;} = string.Empty;
    public DateTimeOffset? LastReviewDate { get; init; }//thiếu thuộc tính 
    public DateTimeOffset? NextReviewDate { get; init; } // thiếu thuộc tính trong db
    public string AssignedNurseName { get; init; } = string.Empty;//thiếu thuộc tính trong db
}