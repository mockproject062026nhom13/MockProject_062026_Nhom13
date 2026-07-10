using MediatR;

namespace NursingHome.Application.Features.UserSecurity.Commands.Login;

public record LoginResponse(string AccessToken, string TokenType, int ExpiresIn);

public record LoginCommand(string EmployeeCode, string Password) : IRequest<LoginResponse>;
