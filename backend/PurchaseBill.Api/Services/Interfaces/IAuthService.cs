using PurchaseBill.Api.DTOs.Auth;

namespace PurchaseBill.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ExternalLoginResponseDto> LoginAsync(
        LoginRequestDto loginRequest
    );
}