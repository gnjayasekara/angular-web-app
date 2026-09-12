using PurchaseBill.Api.Configuration;
using PurchaseBill.Api.Services;
using PurchaseBill.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add controller support
builder.Services.AddControllers();

// Configure external API settings
builder.Services.Configure<ExternalApiSettings>(
    builder.Configuration.GetSection("ExternalApi"));

// Register HttpClient for external API calls
builder.Services.AddHttpClient();

// Register authentication service
builder.Services.AddScoped<IAuthService, AuthService>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Development OpenAPI endpoint
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map controller routes such as /api/auth/login
app.MapControllers();

app.Run();