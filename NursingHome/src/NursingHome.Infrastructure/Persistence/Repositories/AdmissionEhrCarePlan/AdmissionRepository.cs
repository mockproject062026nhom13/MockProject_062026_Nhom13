using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories.AdmissionEhrCarePlan;

public class AdmissionRepository : IAdmissionRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public AdmissionRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PreAdmissionScreeningDetailDto?> GetPreAdmissionScreeningDetailAsync(
        long screeningId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.PreAdmissionScreenings
            .AsNoTracking()
            .Where(screening => screening.Id == screeningId)
            .Select(screening => new PreAdmissionScreeningDetailDto
            {
                ScreeningId = screening.Id,
                Status = screening.Status,
                CreatedAt = screening.CreatedAt,
                Resident = new ResidentSummaryDto
                {
                    ResidentId = screening.Resident.Id,
                    FullName = screening.Resident.MiddleName == null || screening.Resident.MiddleName == ""
                        ? screening.Resident.FirstName + " " + screening.Resident.LastName
                        : screening.Resident.FirstName + " " + screening.Resident.MiddleName + " " + screening.Resident.LastName,
                    DateOfBirth = screening.Resident.DateOfBirth,
                    Gender = screening.Resident.Gender,
                    Status = screening.Resident.Status
                },
                ScreenedBy = new UserSummaryDto
                {
                    UserId = screening.ScreenedByNavigation.Id,
                    FullName = screening.ScreenedByNavigation.MiddleName == null || screening.ScreenedByNavigation.MiddleName == ""
                        ? screening.ScreenedByNavigation.FirstName + " " + screening.ScreenedByNavigation.LastName
                        : screening.ScreenedByNavigation.FirstName + " " + screening.ScreenedByNavigation.MiddleName + " " + screening.ScreenedByNavigation.LastName,
                    Email = screening.ScreenedByNavigation.Email,
                    RoleId = screening.ScreenedByNavigation.RoleId,
                    RoleName = screening.ScreenedByNavigation.Role.RoleName
                }
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
