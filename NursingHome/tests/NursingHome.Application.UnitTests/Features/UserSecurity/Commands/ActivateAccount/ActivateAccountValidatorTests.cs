using FluentValidation.TestHelper;
using NursingHome.Application.Features.UserSecurity.Commands;
using NursingHome.Application.Features.UserSecurity.Validators;
using Xunit;

namespace NursingHome.Application.UnitTests.Features.UserSecurity.Validators;

public class ActivateAccountValidatorTests
{
    private readonly ActivateAccountValidator _validator;

    public ActivateAccountValidatorTests()
    {
        _validator = new ActivateAccountValidator();
    }

    [Fact]
    public void Validate_Should_NotHaveErrors_When_CommandIsValid()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Should_HaveError_When_EmailIsEmpty()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Email = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_EmailFormatIsInvalid()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Email = "invalid-email";

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email format is invalid.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_PasswordIsEmpty()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Password = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_PasswordIsTooShort()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Password = "Ab1";

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_PasswordHasNoUppercase()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Password = "password123";

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_PasswordHasNoLowercase()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Password = "PASSWORD123";

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_PasswordHasNoNumber()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Password = "Password";

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one number.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_ConfirmPasswordIsEmpty()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ConfirmPassword = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
            .WithErrorMessage("Confirm password is required.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_ConfirmPasswordDoesNotMatch()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ConfirmPassword = "Different123";

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
            .WithErrorMessage("Confirm password must match password.");
    }

    [Fact]
    public void Validate_Should_HaveError_When_PhoneNumberIsEmpty()
    {
        // Arrange
        var command = CreateValidCommand();
        command.PhoneNumber = string.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Phone number is required to activate your account.");
    }

    [Theory]
    [InlineData("0901234567")]
    [InlineData("84901234567")]
    [InlineData("+0123456789")]
    [InlineData("+84-901234567")]
    [InlineData("abc")]
    public void Validate_Should_HaveError_When_PhoneNumberIsNotInE164Format(string phoneNumber)
    {
        // Arrange
        var command = CreateValidCommand();
        command.PhoneNumber = phoneNumber;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Phone number must be in E.164 format (example: +84901234567).");
    }

    [Theory]
    [InlineData("+84901234567")]
    [InlineData("+14155552671")]
    [InlineData("+819012345678")]
    public void Validate_Should_NotHaveError_When_PhoneNumberIsValidE164(string phoneNumber)
    {
        // Arrange
        var command = CreateValidCommand();
        command.PhoneNumber = phoneNumber;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    private static ActivateAccountCommand CreateValidCommand()
    {
        return new ActivateAccountCommand
        {
            Email = "test@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
            PhoneNumber = "+84901234567"
        };
    }
}