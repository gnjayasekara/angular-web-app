using Microsoft.AspNetCore.Mvc;
using PurchaseBill.Api.DTOs.Auth;
using PurchaseBill.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

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

    
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            authenticated = true
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequest)
    {
        try
        {
            var result = await _authService.LoginAsync(loginRequest);

            Response.Cookies.Append(
                "access_token",
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // local HTTP only
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                }
            );

            return Ok(new
            {
                result.UserCode,
                result.DisplayName,
                result.Email,
                result.CompanyCode,
                result.Locations
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
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
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            "access_token",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            }
        );

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }
}