using PurchaseBill.Api.DTOs.Auth;

namespace PurchaseBill.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
}