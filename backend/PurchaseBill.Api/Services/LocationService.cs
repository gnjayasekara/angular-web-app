using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Data;
using PurchaseBill.Api.DTOs.Auth;
using PurchaseBill.Api.Models;
using PurchaseBill.Api.Services.Interfaces;

namespace PurchaseBill.Api.Services;

public class LocationService : ILocationService
{
    private readonly AppDbContext _dbContext;

    public LocationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SyncLocationsAsync(
        string companyCode,
        IEnumerable<UserLocationDto> locations)
    {
        var locationList = locations.ToList();

        var locationCodes = locationList
            .Select(x => x.LocationCode)
            .ToList();

        var existingLocations = await _dbContext.LocationDetails
            .Where(x =>
                x.CompanyCode == companyCode &&
                locationCodes.Contains(x.LocationCode))
            .ToDictionaryAsync(x => x.LocationCode);

        foreach (var location in locationList)
        {
            if (existingLocations.TryGetValue(
                location.LocationCode,
                out var existingLocation))
            {
                existingLocation.LocationName = location.LocationName;
                existingLocation.StockHandle = location.StockHandle;
                existingLocation.Address = location.Address;
                existingLocation.Phone = location.Phone;
                existingLocation.Status = location.Status;

                continue;
            }

            var newLocation = new LocationDetail
            {
                CompanyCode = companyCode,
                LocationCode = location.LocationCode,
                LocationName = location.LocationName,
                StockHandle = location.StockHandle,
                Address = location.Address,
                Phone = location.Phone,
                Status = location.Status
            };

            _dbContext.LocationDetails.Add(newLocation);
        }

        await _dbContext.SaveChangesAsync();
    }
}