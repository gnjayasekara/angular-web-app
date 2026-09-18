using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Data;
using PurchaseBill.Api.DTOs.Dashboard;
using PurchaseBill.Api.Services.Interfaces;

namespace PurchaseBill.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardResponseDto> GetDashboardAsync()
    {
        // Widget 1: Latest 5 purchase orders.
        var latestOrders = await _context.PurchaseBills
            .AsNoTracking()
            .OrderByDescending(bill => bill.CreatedAt)
            .ThenByDescending(bill => bill.Id)
            .Take(5)
            .Select(bill => new LatestPurchaseOrderDto
            {
                Id = bill.Id,
                NetAmount = bill.Items
                    .Sum(item => (decimal?)item.TotalCost) ?? 0m,
                NumberOfItems = bill.Items.Count
            })
            .ToListAsync();

        // Widget 2: Oldest 10 purchase order item rows.
        var oldestOrderItems = await _context.PurchaseBillItems
            .AsNoTracking()
            .OrderBy(item => item.PurchaseBill.CreatedAt)
            .ThenBy(item => item.Id)
            .Take(10)
            .Select(item => new OldestPurchaseOrderItemDto
            {
                PurchaseOrderId = item.PurchaseBillId,
                ItemName = item.ItemName,
                Quantity = item.Quantity
            })
            .ToListAsync();

        // Widget 3: Total quantity for each item name across all orders.
        var itemQuantities = await _context.PurchaseBillItems
            .AsNoTracking()
            .GroupBy(item => item.ItemName)
            .Select(group => new ItemQuantityDto
            {
                ItemName = group.Key,
                Quantity = group.Sum(item => (long)item.Quantity)
            })
            .OrderBy(item => item.ItemName)
            .ToListAsync();

        return new DashboardResponseDto
        {
            LatestOrders = latestOrders,
            OldestOrderItems = oldestOrderItems,
            ItemQuantities = itemQuantities
        };
    }
}