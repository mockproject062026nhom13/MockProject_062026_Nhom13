namespace NursingHome.Application.Features.UserSecurity.DTOs;

// SC_004 — 4 thẻ đếm ở đầu màn hình User List.
public record UserStatisticsDto(
    int Total,
    int Active,
    int Invited,
    int SuspendedOrDeactivated
);
