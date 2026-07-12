using MediatR;
using Microsoft.Extensions.Configuration;
using NursingHome.Application.Abstractions.Authentication;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.UserSecurity.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IJwtTokenService jwtTokenService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy thông tin user dựa vào EmployeeCode
        var user = await _userRepository.GetAuthUserByEmployeeCodeAsync(request.EmployeeCode, cancellationToken);

        // 2. Validate tài khoản tồn tại và kiểm tra password
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new DomainException("Invalid EmployeeCode or password.");
        }

        // 3. Kiểm tra trạng thái tài khoản
        if (user.Status != NursingHome.Domain.Constants.UserStatuses.Active)
        {
            throw new DomainException($"Account is {user.Status.ToLower()}. Please contact administrator.");
        }

        // 4. Sinh JWT Token
        var token = _jwtTokenService.GenerateToken(user);

        // 5. Cập nhật LastLoginAt
        await _userRepository.UpdateLastLoginAsync(user.Id, cancellationToken);

        var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var exp) ? exp : 60;

        return new LoginResponse(token, "Bearer", expirationMinutes * 60);
    }
}
