using System.ComponentModel.DataAnnotations;

namespace PurchaseBill.Api.DTOs.PurchaseBills;

public class CreatePurchaseBillDto
{
    [Required]
    [MinLength(1)]
    public List<CreatePurchaseBillItemDto> Items { get; set; } = new();
}