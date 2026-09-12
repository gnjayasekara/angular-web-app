using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Data;
using PurchaseBill.Api.DTOs.PurchaseBills;
using PurchaseBill.Api.Models;
using PurchaseBill.Api.Services.Interfaces;
using PurchaseBillModel = PurchaseBill.Api.Models.PurchaseBill;

namespace PurchaseBill.Api.Services;

public class PurchaseBillService : IPurchaseBillService
{
    private readonly AppDbContext _dbContext;

    public PurchaseBillService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PurchaseBillResponseDto>> GetAllAsync()
    {
        return await _dbContext.PurchaseBills
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new PurchaseBillResponseDto
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                TotalItems = x.Items.Count,
                TotalQuantity = x.Items.Sum(item => item.Quantity),

                Items = x.Items.Select(item =>
                    new PurchaseBillItemResponseDto
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        LocationCode = item.LocationCode,
                        BatchName = item.BatchName,
                        StandardCost = item.StandardCost,
                        StandardPrice = item.StandardPrice,
                        Quantity = item.Quantity,
                        DiscountPercentage = item.DiscountPercentage,
                        TotalCost = item.TotalCost,
                        TotalSelling = item.TotalSelling
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<PurchaseBillResponseDto?> GetByIdAsync(int id)
    {
        return await _dbContext.PurchaseBills
            .Where(x => x.Id == id)
            .Select(x => new PurchaseBillResponseDto
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                TotalItems = x.Items.Count,
                TotalQuantity = x.Items.Sum(item => item.Quantity),

                Items = x.Items.Select(item =>
                    new PurchaseBillItemResponseDto
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        LocationCode = item.LocationCode,
                        BatchName = item.BatchName,
                        StandardCost = item.StandardCost,
                        StandardPrice = item.StandardPrice,
                        Quantity = item.Quantity,
                        DiscountPercentage = item.DiscountPercentage,
                        TotalCost = item.TotalCost,
                        TotalSelling = item.TotalSelling
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PurchaseBillResponseDto> CreateAsync(
        CreatePurchaseBillDto request)
    {
        var purchaseBill = new PurchaseBillModel
        {
            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in request.Items)
        {
            var location = await _dbContext.LocationDetails
                .FirstOrDefaultAsync(x =>
                    x.LocationCode == item.LocationCode);

            if (location is null)
            {
                throw new ArgumentException(
                    $"Invalid location: {item.LocationCode}"
                );
            }

            var baseCost =
                item.StandardCost * item.Quantity;

            var discountAmount =
                baseCost * item.DiscountPercentage / 100;

            var totalCost =
                baseCost - discountAmount;

            var totalSelling =
                item.StandardPrice * item.Quantity;

            var purchaseBillItem = new PurchaseBillItem
            {
                ItemName = item.ItemName.Trim(),
                LocationCode = location.LocationCode,
                BatchName = location.LocationName,
                StandardCost = item.StandardCost,
                StandardPrice = item.StandardPrice,
                Quantity = item.Quantity,
                DiscountPercentage = item.DiscountPercentage,
                TotalCost = totalCost,
                TotalSelling = totalSelling
            };

            purchaseBill.Items.Add(purchaseBillItem);
        }

        _dbContext.PurchaseBills.Add(purchaseBill);

        await _dbContext.SaveChangesAsync();

        return new PurchaseBillResponseDto
        {
            Id = purchaseBill.Id,
            CreatedAt = purchaseBill.CreatedAt,
            TotalItems = purchaseBill.Items.Count,
            TotalQuantity =
                purchaseBill.Items.Sum(x => x.Quantity),

            Items = purchaseBill.Items.Select(x =>
                new PurchaseBillItemResponseDto
                {
                    Id = x.Id,
                    ItemName = x.ItemName,
                    LocationCode = x.LocationCode,
                    BatchName = x.BatchName,
                    StandardCost = x.StandardCost,
                    StandardPrice = x.StandardPrice,
                    Quantity = x.Quantity,
                    DiscountPercentage = x.DiscountPercentage,
                    TotalCost = x.TotalCost,
                    TotalSelling = x.TotalSelling
                })
                .ToList()
        };
    }
}