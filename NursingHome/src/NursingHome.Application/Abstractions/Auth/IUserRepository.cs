using NursingHome.Application.Features.Auth.DTOs;

namespace NursingHome.Application.Abstractions.Auth;

public interface IUserRepository
{
    Task<UserAuthDto?> GetUserAuthInfoByEmailAsync(string email);
    Task UpdateLoginTimeAsync(long userId);
}