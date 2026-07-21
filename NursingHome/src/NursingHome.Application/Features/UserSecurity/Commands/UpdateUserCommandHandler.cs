using MediatR;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Abstractions.UserSecurity;
using NursingHome.Application.Common;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.UserSecurity.Commands;

public class UpdateUserCommandHandler(
    IUserAdminRepository userAdminRepository,
    IUserRepository userRepository)
    : IRequestHandler<UpdateUserCommand, ApiResponse<bool>>
{
    private readonly IUserAdminRepository _userAdminRepository = userAdminRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ApiResponse<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var roleExists = await _userRepository.IsRoleExistsAsync(request.RoleId, cancellationToken);
        if (!roleExists)
            throw new DomainException("The specified Role ID does not exist.");

        var (firstName, middleName, lastName) = SplitFullName(request.FullName);

        var updated = await _userAdminRepository.UpdateProfileAsync(
            request.Id,
            firstName,
            middleName,
            lastName,
            NormalizePhoneNumber(request.PhoneNumber),
            string.IsNullOrWhiteSpace(request.LicenseNumber) ? null : request.LicenseNumber.Trim(),
            request.RoleId,
            cancellationToken);

        if (!updated)
            throw new NotFoundException($"Không tìm thấy người dùng có ID là {request.Id}.");

        return ApiResponse<bool>.CreateSuccess(true, 200, "Cập nhật người dùng thành công.");
    }

    private static (string First, string? Middle, string Last) SplitFullName(string fullName)
    {
        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return parts.Length switch
        {
            > 2 => (parts[0], string.Join(' ', parts.Skip(1).Take(parts.Length - 2)), parts[^1]),
            2 => (parts[0], null, parts[1]),
            1 => (parts[0], null, ""),
            _ => ("", null, "")
        };
    }

    // Giữ cùng quy tắc chuẩn hóa như CreateUserCommandHandler.
    private static string? NormalizePhoneNumber(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var digits = new string(input.Where(char.IsDigit).ToArray());

        if (digits.Length == 10)
            digits = "1" + digits;

        if (digits.Length == 11 && digits.StartsWith('1'))
            return $"{digits[0]}-{digits.Substring(1, 3)}-{digits.Substring(4, 3)}-{digits.Substring(7, 4)}";

        return input;
    }
}
