using System.ComponentModel.DataAnnotations;

namespace PurchaseBill.Api.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Company code is required.")]
    public string CompanyCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}