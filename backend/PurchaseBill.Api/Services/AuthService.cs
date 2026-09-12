using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PurchaseBill.Api.Configuration;
using PurchaseBill.Api.DTOs.Auth;
using PurchaseBill.Api.Services.Interfaces;

namespace PurchaseBill.Api.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApiSettings _externalApiSettings;

    public AuthService(
        HttpClient httpClient,
        IOptions<ExternalApiSettings> externalApiOptions)
    {
        _httpClient = httpClient;
        _externalApiSettings = externalApiOptions.Value;
    }

    public async Task<ExternalLoginResponseDto> LoginAsync(
        LoginRequestDto loginRequest)
    {
        var url =
            $"{_externalApiSettings.BaseUrl.TrimEnd('/')}" +
            $"{_externalApiSettings.LoginEndpoint}";

        var externalRequest = new ExternalLoginRequestDto
        {
            ApiAction = "GetLoginData",
            DeviceId = "D001",
            SyncTime = "",

            CompanyCode = loginRequest.CompanyCode.Trim(),

            ApiBody = new ExternalLoginBodyDto
            {
                Username = loginRequest.Username.Trim(),
                Password = loginRequest.Password
            }
        };

        var json = JsonSerializer.Serialize(externalRequest);

        Console.WriteLine($"External API URL: {url}");

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            url
        );

        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("*/*")
        );

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.SendAsync(request);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine($"HTTP Status: {response.StatusCode}");
        Console.WriteLine($"External Response: {responseBody}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"External API returned HTTP {(int)response.StatusCode}."
            );
        }

        var loginResponse =
            JsonSerializer.Deserialize<ExternalLoginResponseDto>(
                responseBody
            );

        if (loginResponse is null)
        {
            throw new InvalidOperationException(
                "External API returned an invalid response."
            );
        }

        if (loginResponse.StatusCode != 200)
        {
            throw new UnauthorizedAccessException(
                loginResponse.Message ?? "Login failed."
            );
        }

        return loginResponse;
    }
}