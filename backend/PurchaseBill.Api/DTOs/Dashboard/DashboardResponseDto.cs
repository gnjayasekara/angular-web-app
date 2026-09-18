namespace PurchaseBill.Api.DTOs.Dashboard;

public class DashboardResponseDto
{
    public List<LatestPurchaseOrderDto> LatestOrders { get; set; } = new();

    public List<OldestPurchaseOrderItemDto> OldestOrderItems { get; set; } = new();

    public List<ItemQuantityDto> ItemQuantities { get; set; } = new();
}

public class LatestPurchaseOrderDto
{
    public int Id { get; set; }

    public decimal NetAmount { get; set; }

    public int NumberOfItems { get; set; }
}

public class OldestPurchaseOrderItemDto
{
    public int PurchaseOrderId { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public int Quantity { get; set; }
}

public class ItemQuantityDto
{
    public string ItemName { get; set; } = string.Empty;

    public long Quantity { get; set; }
}