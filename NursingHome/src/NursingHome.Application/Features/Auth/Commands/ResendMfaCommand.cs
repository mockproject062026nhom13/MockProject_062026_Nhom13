using MediatR;

namespace NursingHome.Application.Features.Auth.Commands;

public record ResendMfaCommand(string Email):IRequest<bool>;