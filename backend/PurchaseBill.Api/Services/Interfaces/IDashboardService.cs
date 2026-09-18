using PurchaseBill.Api.DTOs.Dashboard;

namespace PurchaseBill.Api.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponseDto> GetDashboardAsync();
}