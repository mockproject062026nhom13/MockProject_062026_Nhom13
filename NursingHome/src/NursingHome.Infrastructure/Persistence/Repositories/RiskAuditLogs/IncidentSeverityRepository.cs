using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.RiskAuditLogs;

public class IncidentSeverityRepository(NursingHomeDbContext context) : IIncidentSeverityRepository
{
    private readonly NursingHomeDbContext _context = context;
    //function to retrieve data from the database
    public async Task<List<IncidentSeverityDto>> GetAllAsync()
    {
        //get the IncidentSeverity table
        var listFromDb = await _context.Set<IncidentSeverity>()
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync();

        //convert data from Entity to DTO
        var listDto = listFromDb.Select(x => new IncidentSeverityDto(
            Id: x.Id,
            LevelName: x.LevelName
        // Description: x.Description,
        // Example: x.Example
        )).ToList();

        return listDto;
    }
    //function to update incident types
    public async Task<bool> UpdateDescriptionAndExampleAsync(long id, string? description, string? example)
    {
        var entity = await _context.Set<IncidentSeverity>().FindAsync(id);

        if (entity == null) return false;

        // entity.UpdateDetails(description, example);

        await _context.SaveChangesAsync();

        return true;
    }


}