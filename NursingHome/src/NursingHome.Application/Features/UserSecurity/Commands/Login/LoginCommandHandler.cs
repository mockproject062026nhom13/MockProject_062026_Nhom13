using MediatR;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Abstractions.Authentication;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Domain.Constants;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.UserSecurity.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IOtpService otpService,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _otpService = otpService;
        _emailService = emailService;
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

        // 5. Generate and Send OTP
        var otp = await _otpService.GenerateOtpAsync(user.Id, cancellationToken);
        var subject = "Your Login Verification Code";
        var body = $"<p>Your 2-Step Verification code is: <strong>{otp}</strong></p><p>This code will expire in 5 minutes.</p>";

        // In a real scenario, you'd check if request.Identifier is email or phone.
        // Assuming Email for this SmtpEmailService implementation.
        await _emailService.SendEmailAsync(user.Email, subject, body, cancellationToken);

        // 6. Do NOT update LastLoginAt yet (will be updated after 2FA)

        return new LoginResponse(true, preAuthToken, "Bearer", 5 * 60);
    }
}
