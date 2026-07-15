using NursingHome.Domain.Entities;
using NursingHome.Domain.Enums;

namespace NursingHome.Domain.Factories;

public static class UserFactory
{
    public static User Hydrate(
        long id,
        string employeeCode,
        string email,
        string passwordHash,
        string firstName,
        string? middleName,
        string lastName,
        string? licenseNumber,
        string? phoneNumber,
        UserStatus status,
        bool mfaEnabled,
        DateTimeOffset? lastLoginAt,
        long roleId,
        bool isDeleted,
        DateTimeOffset? deletedAt,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        return new User(
            id,
            employeeCode,
            email,
            passwordHash,
            firstName,
            middleName,
            lastName,
            licenseNumber,
            phoneNumber,
            status,
            mfaEnabled,
            lastLoginAt,
            roleId,
            isDeleted,
            deletedAt,
            createdAt,
            updatedAt);
    }
}