namespace PurchaseBill.Api.Models;

public class PurchaseBill
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<PurchaseBillItem> Items { get; set; } = new();
}