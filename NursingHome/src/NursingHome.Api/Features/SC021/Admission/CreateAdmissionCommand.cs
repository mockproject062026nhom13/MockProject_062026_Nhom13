namespace NursingHome.Application.Features.Admission.CreateAdmission;

public record CreateAdmissionCommand(
    long FacilityId,
    long RoomId,
    long BedId,
    DateTime AdmissionDate,
    long ResidentId,
    long UserId,
    string ProviderName,
    string ProviderType
);
