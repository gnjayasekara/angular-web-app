namespace PurchaseBill.Api.DTOs.PurchaseBills;

public class PurchaseBillResponseDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public int TotalItems { get; set; }

    public int TotalQuantity { get; set; }

    public List<PurchaseBillItemResponseDto> Items { get; set; } = new();
}

public class PurchaseBillItemResponseDto
{
    public int Id { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string LocationCode { get; set; } = string.Empty;

    public string BatchName { get; set; } = string.Empty;

    public decimal StandardCost { get; set; }

    public decimal StandardPrice { get; set; }

    public int Quantity { get; set; }

    public decimal DiscountPercentage { get; set; }

    public decimal TotalCost { get; set; }

    public decimal TotalSelling { get; set; }
}