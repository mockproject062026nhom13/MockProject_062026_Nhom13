namespace NursingHome.Application.Features.CareLevelResidents.DTOs;

public record ResidentListDto(
    long Id,
    string FullName,
    string DobWithAge, // return with format "MM/dd/yyyy (Age)"
    string Status,
    string Room,// return with format "room-bed"
    //string? ReferralSource,
    string? PayerSource
);