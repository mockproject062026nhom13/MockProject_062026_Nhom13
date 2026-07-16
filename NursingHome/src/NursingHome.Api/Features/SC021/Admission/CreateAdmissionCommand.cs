namespace NursingHome.Api.Features.CreateAdmission;

public record CreateAdmissionCommand(
    long FacilityId,
    long RoomId,
    long BedId,
    DateOnly AdmissionDate,
    long ResidentId,
    long UserId
);
