using Moq;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Abstractions.Authentication;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.Commands;
using NursingHome.Domain.Entities;
using NursingHome.Domain.Enums;
using Xunit;

namespace NursingHome.Application.UnitTests.Features.UserSecurity.Commands.ActivateAccount;

public class ActivateAccountCommandHandlerTests
{
    private readonly Mock<IActivateAccountRepository> _repositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;

    private readonly ActivateAccountCommandHandler _handler;


    public ActivateAccountCommandHandlerTests()
    {
        _repositoryMock = new Mock<IActivateAccountRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();

        _handler = new ActivateAccountCommandHandler(
            _repositoryMock.Object,
            _passwordHasherMock.Object);
    }


    [Fact]
    public async Task Handle_Should_ReturnNotFound_When_UserDoesNotExist()
    {
        // Arrange
        var command = new ActivateAccountCommand
        {
            Email = "unknown@test.com",
            Password = "Password123",
            PhoneNumber = "0123456789"
        };


        _repositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);


        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);


        // Assert
        Assert.False(result.Success);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(
            "User not found.",
            result.Message);


        _repositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<User>()),
            Times.Never);


        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }


    [Fact]
    public async Task Handle_Should_ActivateAccountSuccessfully_When_UserExists()
    {
        // Arrange
        var user = CreateUser();

        var command = new ActivateAccountCommand
        {
            Email = user.Email,
            Password = "Password123",
            ConfirmPassword = "Password123",
            PhoneNumber = "0987654321"
        };


        _repositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(user);


        _passwordHasherMock
            .Setup(x => x.HashPassword(command.Password))
            .Returns("hashed_password");


        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);


        // Assert

        Assert.True(result.Success);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(
            "Account activated successfully.",
            result.Message);


        Assert.NotNull(result.Data);

        Assert.Equal(
            user.Email,
            result.Data!.Email);


        Assert.Equal(
            UserStatus.ACTIVE,
            user.Status);


        Assert.Equal(
            "hashed_password",
            user.PasswordHash);


        Assert.Equal(
            command.PhoneNumber,
            user.PhoneNumber);



        _passwordHasherMock.Verify(
            x => x.HashPassword(command.Password),
            Times.Once);


        _repositoryMock.Verify(
            x => x.UpdateAsync(user),
            Times.Once);


        _repositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }


    private static User CreateUser()
    {
        return new User(
            employeeCode: "EMP001",
            email: "test@test.com",
            firstName: "John",
            lastName: "Doe",
            roleId: 1);
    }
}