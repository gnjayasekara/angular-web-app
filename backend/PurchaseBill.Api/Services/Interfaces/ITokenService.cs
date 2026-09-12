using PurchaseBill.Api.DTOs.Auth;

namespace PurchaseBill.Api.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(ExternalUserDto user);
}