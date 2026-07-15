
using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Abstractions.Authentication;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.DTOs;

namespace NursingHome.Application.Features.UserSecurity.Commands.ActivateAccount;

public class ActivateAccountCommandHandler
    : IRequestHandler<
        ActivateAccountCommand,
        ApiResponse<ActivateAccountResponse>>
{
    private readonly IActivateAccountRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public ActivateAccountCommandHandler(
        IActivateAccountRepository repository,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<ActivateAccountResponse>> Handle(
        ActivateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ApiResponse<ActivateAccountResponse>.CreateError(
                404,
                "User not found.");
        }
        // in domain layer, we have already check if user is deleted, suspended, locked or active, so we don't need to check here again
        user.ActivateAccount(
            _passwordHasher.HashPassword(request.Password),
            request.PhoneNumber);


        await _repository.UpdateAsync(user);
        await _repository.SaveChangesAsync();

        return ApiResponse<ActivateAccountResponse>.CreateSuccess(
            new ActivateAccountResponse
            {
                Email = user.Email,
                Status = user.Status.ToString()
            },
            200,
            "Account activated successfully.");
    }
}