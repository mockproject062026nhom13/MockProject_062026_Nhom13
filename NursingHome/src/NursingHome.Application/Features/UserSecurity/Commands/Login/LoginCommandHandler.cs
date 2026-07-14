using MediatR;
using Microsoft.Extensions.Configuration;
using NursingHome.Application.Abstractions.Authentication;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Domain.Constants;
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
        // 1. Lấy thông tin user dựa vào Identifier (Email/Phone)
        var user = await _userRepository.GetAuthUserByIdentifierAsync(request.Identifier, cancellationToken);

        // 2. Validate tài khoản tồn tại và kiểm tra password
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new DomainException("Invalid identifier or password.");
        }

        // 3. Kiểm tra trạng thái tài khoản
        if (user.Status.Equals("Invited", StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException("Account not activated, check invite link.");
        }
        else if (user.Status.Equals("Suspended", StringComparison.OrdinalIgnoreCase) || 
                 user.Status.Equals("Deactivated", StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException("Account is suspended or deactivated. Please contact your Admin.");
        }
        else if (user.Status != UserStatuses.Active)
        {
            throw new DomainException($"Account is {user.Status.ToLower()}. Please contact administrator.");
        }

        // 4. Generate Pre-Auth Token for 2FA
        var preAuthToken = _jwtTokenService.GeneratePreAuthToken(user);

        // 5. Do NOT update LastLoginAt yet (will be updated after 2FA)

        return new LoginResponse(true, preAuthToken, "Bearer", 5 * 60);
    }
}
