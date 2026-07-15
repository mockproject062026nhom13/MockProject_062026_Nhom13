namespace NursingHome.Application.Abstractions;

public interface ICurrentUserService
{
    long? UserId { get; }

    string? IpAddress { get; }
}