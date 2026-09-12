using System.Text.Json.Serialization;
using System.Text.Json;

namespace PurchaseBill.Api.DTOs.Auth;

public class ExternalLoginResponseDto
{
    [JsonPropertyName("Status_Code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("Sync_Time")]
    public string? SyncTime { get; set; }

    [JsonPropertyName("Message")]
    public string? Message { get; set; }

    [JsonPropertyName("Response_Body")]
    public List<ExternalUserDto>? ResponseBody { get; set; }
}

public class ExternalUserDto
{
    [JsonPropertyName("User_Code")]
    public string UserCode { get; set; } = string.Empty;

    [JsonPropertyName("User_Display_Name")]
    public string UserDisplayName { get; set; } = string.Empty;

    [JsonPropertyName("Email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("User_Employee_Code")]
    public string UserEmployeeCode { get; set; } = string.Empty;

    [JsonPropertyName("Company_Code")]
    public string CompanyCode { get; set; } = string.Empty;

    [JsonPropertyName("User_Locations")]
    public List<UserLocationDto> UserLocations { get; set; } = new();

    [JsonPropertyName("User_Permissions")]
    public List<UserPermissionDto> UserPermissions { get; set; } = new();
}

public class UserLocationDto
{
    [JsonPropertyName("Location_Code")]
    public string LocationCode { get; set; } = string.Empty;

    [JsonPropertyName("Location_Name")]
    public string LocationName { get; set; } = string.Empty;

    [JsonPropertyName("Stock_Handle")]
    public int StockHandle { get; set; }

    [JsonPropertyName("Address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("Phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("Status")]
    public int Status { get; set; }
}

public class UserPermissionDto
{
    [JsonPropertyName("Permisson_Name")]
    public string PermissionName { get; set; } = string.Empty;

    [JsonPropertyName("Permission_Status")]
    public string PermissionStatus { get; set; } = string.Empty;
}