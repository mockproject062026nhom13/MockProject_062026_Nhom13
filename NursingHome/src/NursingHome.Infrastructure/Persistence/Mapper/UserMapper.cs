using NursingHome.Domain.Enums;
using NursingHome.Domain.Factories;

namespace NursingHome.Infrastructure.Persistence.Mappers;

public static class UserMapper
{
    public static Domain.Entities.User ToDomain(
        this Generated.User entity)
    {
        return UserFactory.Hydrate(
            entity.Id,
            entity.EmployeeCode,
            entity.Email,
            entity.PasswordHash,
            entity.FirstName,
            entity.MiddleName,
            entity.LastName,
            entity.LicenseNumber,
            entity.PhoneNumber,
            Enum.Parse<UserStatus>(entity.Status, true),
            entity.MfaEnabled,
            entity.LastLoginAt,
            entity.RoleId,
            entity.IsDeleted,
            entity.DeletedAt,
            entity.CreatedAt,
            entity.UpdatedAt);
    }

    public static void UpdatePersistence(
        this Generated.User entity,
        Domain.Entities.User domain)
    {   
        entity.ApplyFromDomain(domain);
    }
}