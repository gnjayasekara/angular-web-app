using PurchaseBill.Api.DTOs.Auth;

namespace PurchaseBill.Api.Services.Interfaces;

public interface ILocationService
{
    Task SyncLocationsAsync(
        string companyCode,
        IEnumerable<UserLocationDto> locations
    );
}