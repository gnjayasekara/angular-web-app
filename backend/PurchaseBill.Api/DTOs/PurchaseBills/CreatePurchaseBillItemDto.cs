using System.ComponentModel.DataAnnotations;

namespace PurchaseBill.Api.DTOs.PurchaseBills;

public class CreatePurchaseBillItemDto
{
    [Required]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    public string LocationCode { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal StandardCost { get; set; }

    [Range(0, double.MaxValue)]
    public decimal StandardPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, 100)]
    public decimal DiscountPercentage { get; set; }
}