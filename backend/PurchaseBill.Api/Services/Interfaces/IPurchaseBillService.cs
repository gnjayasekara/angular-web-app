using PurchaseBill.Api.DTOs.PurchaseBills;

namespace PurchaseBill.Api.Services.Interfaces;

public interface IPurchaseBillService
{
    Task<List<PurchaseBillResponseDto>> GetAllAsync();

    Task<PurchaseBillResponseDto?> GetByIdAsync(int id);

    Task<PurchaseBillResponseDto> CreateAsync(
        CreatePurchaseBillDto request
    );
}