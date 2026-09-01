using MediatR;
using NursingHome.Application.Abstractions.Auth;

namespace NursingHome.Application.Features.Auth.Commands;

public class ResendMfaCommandHandler(IUserRepository userRepository,IOtpService otpService):IRequestHandler<ResendMfaCommand, bool>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IOtpService _otpService = otpService;

    public async Task<bool> Handle(ResendMfaCommand request, CancellationToken cancellationToken)
    {
        var userDto = await _userRepository.GetUserAuthInfoByEmailAsync(request.Email);
        if (userDto == null || !userDto.MfaEnabled)
        {
            throw new UnauthorizedAccessException("Yêu cầu không hợp lệ. Vui lòng kiểm tra lại tài khoản ");

        }
        await _otpService.GenerateAndSendOtpAsync(request.Email);

        return true;
    }
}