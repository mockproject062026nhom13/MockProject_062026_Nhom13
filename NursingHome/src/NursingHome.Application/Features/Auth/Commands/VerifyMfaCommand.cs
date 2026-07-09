using MediatR;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Features.Auth.DTOs;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.Auth.Commands;

public record VerifyMfaCommand(string Email, string OtpCode) : IRequest<AuthResultDto>;



