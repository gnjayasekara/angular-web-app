namespace PurchaseBill.Api.Models;

public class LocationDetail
{
    public int Id { get; set; }

    public string LocationCode { get; set; } = string.Empty;

    public string LocationName { get; set; } = string.Empty;

    public int StockHandle { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public int Status { get; set; }

    public string CompanyCode { get; set; } = string.Empty;
}