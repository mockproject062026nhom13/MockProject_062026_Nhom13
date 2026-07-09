namespace NursingHome.Application.Features.Auth.DTOs;
public record AuthResultDto(string AccessToken, string RefreshToken, int ExpiresIn);
