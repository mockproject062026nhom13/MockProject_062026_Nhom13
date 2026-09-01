using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.UserSecurity.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<long>>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private static string? NormalizePhoneNumber(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        var digits = new string(input.Where(char.IsDigit).ToArray());

        // add country code = 1: 4155550101 -> 14155550101
        if (digits.Length == 10)
        {
            digits = "1" + digits;
        }

        // 14155550101 -> 1-415-555-0101
        if (digits.Length == 11 && digits.StartsWith("1"))
        {
            return $"{digits[0]}-{digits.Substring(1, 3)}-{digits.Substring(4, 3)}-{digits.Substring(7, 4)}";
        }

        return input;
    }

    public async Task<ApiResponse<long>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {

        bool isEmailUnique =
            await _userRepository.IsEmailUniqueAsync(
                request.Email,
                cancellationToken);

        if (!isEmailUnique)
        {
            return ApiResponse<long>.CreateError(
                400,
                "Email is already registered in the system.");
        }

        bool isRoleValid = await _userRepository.IsRoleExistsAsync(request.RoleId, cancellationToken);
        if (!isRoleValid)
        {
            return ApiResponse<long>.CreateError(400, "The specified Role ID does not exist.");
        }

        if (request.AssignedFacilityId.HasValue)
        {
            bool isFacilityValid = await _userRepository.IsFacilityExistsAsync(request.AssignedFacilityId.Value, cancellationToken);
            if (!isFacilityValid)
            {
                return ApiResponse<long>.CreateError(
                    400,
                    $"The specified Facility ID does not exist in the system.");
            }
        }

        // Split FullName
        var nameParts = request.FullName
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string firstName = "";
        string? middleName = null;
        string lastName = "";

        if (nameParts.Length > 2)
        {
            firstName = nameParts[0];
            lastName = nameParts[^1];
            middleName = string.Join(
                " ",
                nameParts.Skip(1).Take(nameParts.Length - 2));
        }
        else if (nameParts.Length == 2)
        {
            firstName = nameParts[0];
            lastName = nameParts[1];
        }
        else if (nameParts.Length == 1)
        {
            firstName = nameParts[0];
        }

        // Generate Employee Code
        string lastCode =
            await _userRepository.GetLastEmployeeCodeAsync(cancellationToken)
            ?? "";

        int nextNumber = 1;

        if (lastCode.StartsWith("NHMS-") &&
            int.TryParse(lastCode.Substring(5), out int lastSequence))
        {
            nextNumber = lastSequence + 1;
        }

        string employeeCode = $"NHMS-{nextNumber:D4}";

        var userDto = new UserCreationDto
        {
            EmployeeCode = employeeCode,
            Email = request.Email,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            PhoneNumber = NormalizePhoneNumber(request.PhoneNumber),
            RoleId = request.RoleId,
            AssignedFacilityId = request.AssignedFacilityId
        };

        long newUserId =
            await _userRepository.AddUserAsync(
                userDto,
                cancellationToken);

        return ApiResponse<long>.CreateSuccess(newUserId);
    }
}