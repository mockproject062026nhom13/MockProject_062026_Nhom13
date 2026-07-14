namespace NursingHome.Application.Features.CareLevelResidents.DTOs;

public record ResidentStatisticsDto(
    int TotalResidents, 
    int Active, 
    int Discharged, 
    int Pending,
    int Deceased
);