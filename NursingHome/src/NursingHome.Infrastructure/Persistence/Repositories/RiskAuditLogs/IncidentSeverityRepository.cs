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
        // var listDto = listFromDb.Select(x => new IncidentSeverityDto(
        //     Id: x.Id,
        //     LevelName: x.LevelName,
        //     Description: x.Description,
        //     Example: x.Example
        // )).ToList();

        var listDto = listFromDb.Select(x => 
        {
            // Gọi hàm giả lập dữ liệu dựa theo Id
            var mockData = GetMockDetails(x.Id); 
            
            return new IncidentSeverityDto(
                Id: x.Id,
                LevelName: x.LevelName,
                Description: mockData.Description, // Dữ liệu giả lập
                Example: mockData.Example          // Dữ liệu giả lập
            );
        }).ToList();

        return listDto;
    }
    //function to update incident types
    // public async Task<bool> UpdateDescriptionAndExampleAsync(long id, string? description, string? example)
    // {
    //     var entity = await _context.Set<IncidentSeverity>().FindAsync(id);

    //     if (entity == null) return false;

    //     // entity.UpdateDetails(description, example);

    //     await _context.SaveChangesAsync();

    //     return true;
    // }

    public async Task<bool> UpdateDescriptionAndExampleAsync(long id, string? description, string? example)
    {
        return await Task.FromResult(false); 
    }
    private (string Description, string Example) GetMockDetails(long id)
    {
        return id switch
        {
            1 => ("Mức độ nhẹ, có thể xử lý tại chỗ, không ảnh hưởng sinh hoạt", "Trầy xước da nhẹ, mẩn đỏ dị ứng nhẹ"),
            2 => ("Mức độ trung bình, cần theo dõi y tế và chăm sóc đặc biệt", "Ngã bầm tím, sốt cao kéo dài"),
            3 => ("Mức độ nghiêm trọng, đe dọa tính mạng, cần cấp cứu ngay", "Gãy xương, nhồi máu cơ tim, đột quỵ"),
            _ => ("Mô tả đang chờ cập nhật hệ thống", "Ví dụ đang chờ cập nhật hệ thống") 
        };
    }


}