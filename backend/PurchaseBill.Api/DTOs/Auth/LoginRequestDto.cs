namespace PurchaseBill.Api.DTOs.Auth;

public class LoginRequestDto
{
    public string CompanyCode { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}