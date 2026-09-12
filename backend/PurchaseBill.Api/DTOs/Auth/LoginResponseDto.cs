namespace PurchaseBill.Api.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    
    public string UserCode { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string CompanyCode { get; set; } = string.Empty;

    public List<LoginLocationDto> Locations { get; set; } = new();
}

public class LoginLocationDto
{
    public string LocationCode { get; set; } = string.Empty;

    public string LocationName { get; set; } = string.Empty;

    public int StockHandle { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public int Status { get; set; }
}