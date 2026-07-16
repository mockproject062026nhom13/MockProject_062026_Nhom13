using MediatR;
using NursingHome.Application.Features.Auth.DTOs;


namespace NursingHome.Application.Features.Auth.Commands;

public record VerifyMfaCommand(string Email, string OtpCode) : IRequest<AuthResultDto>;



