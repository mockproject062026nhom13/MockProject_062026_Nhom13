namespace NursingHome.Application.Features.LocationInfrastructure.DTOs;

public record FacilityResponse(
    string FacilityName,
    string FacilityCode,
    string LicenseNumber,
    string TargetState,
    string City);
