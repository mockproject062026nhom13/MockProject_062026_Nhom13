using MediatR;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Features.Auth.DTOs;


namespace NursingHome.Application.Features.Auth.Commands;


public class VerifyMfaCommandHandler(
    IUserRepository userRepository, 
    IOtpService otpService,
    ITokenService tokenService ): IRequestHandler<VerifyMfaCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IOtpService _otpService = otpService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AuthResultDto> Handle(VerifyMfaCommand request, CancellationToken cancellationToken)
    {
        var userDto = await _userRepository.GetUserAuthInfoByEmailAsync(request.Email);
        
        if (userDto == null)
        {
            
            throw new UnauthorizedAccessException("Tài khoản hoặc mã xác thực không chính xác."); 
        }

        if (userDto.MfaEnabled)
        {
            bool isOtpValid = await _otpService.ValidateOtpAsync(request.Email, request.OtpCode);
            if (!isOtpValid)
            {
                throw new UnauthorizedAccessException("Mã xác thực không chính xác hoặc đã hết hạn.");
            }
        }

        
        await _userRepository.UpdateLoginTimeAsync(userDto.Id);

        
        var accessToken = _tokenService.GenerateAccessToken(userDto.Id, userDto.Email);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new AuthResultDto(accessToken, refreshToken, 3600);
    }
}