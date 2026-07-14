using MediatR;

namespace NursingHome.Application.Features.UserSecurity.Commands.Login;

public record LoginResponse(bool RequiresTwoFactor, string PreAuthToken, string TokenType, int ExpiresIn);

public record LoginCommand(string Identifier, string Password) : IRequest<LoginResponse>;
