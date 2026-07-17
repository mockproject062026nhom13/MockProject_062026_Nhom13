using FastEndpoints;
using NursingHome.Application.Features.CareLevelResidents.DTOs;


public record GetAssessmentsCommand(int UserId) : ICommand<IEnumerable<ResidentAssessmentDto>>;
