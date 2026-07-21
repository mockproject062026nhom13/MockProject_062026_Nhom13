using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.LocationInfrastructure.DTOs;
using NursingHome.Application.Features.LocationInfrastructure.Queries;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.LocationInfrastructure;

public class InventoryRepository : IInventoryRepository
{
    private const string InServiceStatus = "IN_SERVICE";
    private const string UnderMaintenanceStatus = "UNDER_MAINTENANCE";

    private readonly NursingHomeDbContext _dbContext;

    public InventoryRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EquipmentSupplyInventoryDto> GetEquipmentSupplyInventoryAsync(
        GetEquipmentSupplyInventoryQuery query,
        CancellationToken cancellationToken)
    {
        string? search = string.IsNullOrWhiteSpace(query.Search)
            ? null
            : query.Search.Trim();

        string? dmeStatus = string.IsNullOrWhiteSpace(query.DmeStatus)
            ? null
            : query.DmeStatus.Trim();

        string? supplyStatus = string.IsNullOrWhiteSpace(query.SupplyStatus)
            ? null
            : query.SupplyStatus.Trim();

        var dmeQuery = ApplyDmeFilters(
            _dbContext.DurableMedicalEquipments.AsNoTracking().Where(equipment => !equipment.IsDeleted),
            query.FacilityId,
            query.CategoryId,
            dmeStatus,
            search);

        var suppliesQuery = ApplySupplyFilters(
            _dbContext.ConsumableSupplies.AsNoTracking().Where(supply => !supply.IsDeleted),
            query.FacilityId,
            query.CategoryId,
            supplyStatus,
            search);

        var summary = await BuildSummaryAsync(dmeQuery, cancellationToken);
        var dmeItems = await BuildDmePagedResultAsync(
            dmeQuery,
            query.DmePage,
            query.DmePageSize,
            cancellationToken);
        var supplies = await BuildSuppliesPagedResultAsync(
            suppliesQuery,
            query.SupplyPage,
            query.SupplyPageSize,
            cancellationToken);

        return new EquipmentSupplyInventoryDto
        {
            Summary = summary,
            DurableMedicalEquipment = dmeItems,
            ConsumableSupplies = supplies
        };
    }

    private static IQueryable<DurableMedicalEquipment> ApplyDmeFilters(
        IQueryable<DurableMedicalEquipment> query,
        long? facilityId,
        long? categoryId,
        string? dmeStatus,
        string? search)
    {
        if (facilityId.HasValue)
        {
            query = query.Where(equipment => equipment.FacilityId == facilityId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(equipment => equipment.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(dmeStatus))
        {
            query = query.Where(equipment => equipment.Status == dmeStatus);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(equipment =>
                equipment.ItemName.Contains(search) ||
                equipment.AssetTag.Contains(search));
        }

        return query;
    }

    private static IQueryable<ConsumableSupply> ApplySupplyFilters(
        IQueryable<ConsumableSupply> query,
        long? facilityId,
        long? categoryId,
        string? supplyStatus,
        string? search)
    {
        if (facilityId.HasValue)
        {
            query = query.Where(supply => supply.FacilityId == facilityId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(supply => supply.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(supplyStatus))
        {
            query = query.Where(supply => supply.Status == supplyStatus);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(supply => supply.ItemName.Contains(search));
        }

        return query;
    }

    private static async Task<DmeSummaryDto> BuildSummaryAsync(
        IQueryable<DurableMedicalEquipment> query,
        CancellationToken cancellationToken)
    {
        return new DmeSummaryDto
        {
            TotalDmeItems = await query.CountAsync(cancellationToken),
            InUseCount = await query.CountAsync(equipment => equipment.Status == InServiceStatus, cancellationToken),
            UnderMaintenanceCount = await query.CountAsync(
                equipment => equipment.Status == UnderMaintenanceStatus,
                cancellationToken),
            TotalAssetValue = await query.SumAsync(
                equipment => (decimal?)equipment.UnitValue,
                cancellationToken) ?? 0
        };
    }

    private static async Task<PagedResultDto<DmeInventoryItemDto>> BuildDmePagedResultAsync(
        IQueryable<DurableMedicalEquipment> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        int totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(equipment => equipment.ItemName)
            .ThenBy(equipment => equipment.AssetTag)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(equipment => new DmeInventoryItemDto
            {
                EquipmentId = equipment.Id,
                ItemName = equipment.ItemName,
                CategoryId = equipment.CategoryId,
                CategoryName = equipment.Category.CategoryName,
                AssetTag = equipment.AssetTag,
                Status = equipment.Status,
                AssignedUserId = equipment.AssignedToUser,
                AssignedUserName = equipment.AssignedToUserNavigation == null
                    ? null
                    : equipment.AssignedToUserNavigation.FirstName + " " + equipment.AssignedToUserNavigation.LastName,
                AssignedResidentId = equipment.AssignedToResident,
                AssignedResidentName = equipment.AssignedToResidentNavigation == null
                    ? null
                    : equipment.AssignedToResidentNavigation.FirstName + " " + equipment.AssignedToResidentNavigation.LastName,
                UnitValue = equipment.UnitValue
            })
            .ToListAsync(cancellationToken);

        return new PagedResultDto<DmeInventoryItemDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = CalculateTotalPages(totalItems, pageSize),
            Items = items
        };
    }

    private static async Task<PagedResultDto<ConsumableSupplyItemDto>> BuildSuppliesPagedResultAsync(
        IQueryable<ConsumableSupply> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        int totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(supply => supply.ItemName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(supply => new ConsumableSupplyItemDto
            {
                SupplyId = supply.Id,
                ItemName = supply.ItemName,
                CategoryId = supply.CategoryId,
                CategoryName = supply.Category.CategoryName,
                StockOnHand = supply.StockOnHand,
                Total = supply.Total,
                ReorderThreshold = supply.ReorderThreshold,
                UnitCost = supply.UnitCost,
                PrivatePayRate = supply.PrivatePayRate,
                Status = supply.Status
            })
            .ToListAsync(cancellationToken);

        return new PagedResultDto<ConsumableSupplyItemDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = CalculateTotalPages(totalItems, pageSize),
            Items = items
        };
    }

    private static int CalculateTotalPages(int totalItems, int pageSize)
    {
        return totalItems == 0
            ? 0
            : (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
