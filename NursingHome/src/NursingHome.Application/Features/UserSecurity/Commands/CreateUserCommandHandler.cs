using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.UserSecurity.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IValidator<CreateUserCommand> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    private static string NormalizePhoneNumber(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
        {
            return null; 
        }
        var digits = new string(input.Where(char.IsDigit).ToArray());

        // 4155550100
        if (digits.Length == 10)
        {
            return $"+1{digits}";
        }

        // 14155550100
        if (digits.Length == 11 && digits.StartsWith("1"))
        {
            return $"+{digits}";
        }

        // +14155550100
        if (input.StartsWith("+"))
        {
            return "+" + digits;
        }

        return input;
    }

    public async Task<ApiResponse<Guid>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult =
            await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ApiResponse<Guid>.CreateError(
                400,
                validationResult.Errors.First().ErrorMessage);
        }

        bool isEmailUnique =
            await _userRepository.IsEmailUniqueAsync(
                request.Email,
                cancellationToken);

        if (!isEmailUnique)
        {
            return ApiResponse<Guid>.CreateError(
                400,
                "Email is already registered in the system.");
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

        Guid newUserId =
            await _userRepository.AddUserAsync(
                userDto,
                cancellationToken);

        return ApiResponse<Guid>.CreateSuccess(newUserId);
    }
}