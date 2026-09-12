using Microsoft.AspNetCore.Mvc;
using PurchaseBill.Api.DTOs.Auth;
using PurchaseBill.Api.Services.Interfaces;

namespace PurchaseBill.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequest)
    {
        try
        {
            var result = await _authService.LoginAsync(loginRequest);

            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                new
                {
                    message = "External authentication service failed.",
                    error = ex.Message
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message
                }
            );
        }
    }
}