using PurchaseBill.Api.Configuration;
using PurchaseBill.Api.Services;
using PurchaseBill.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add controller support
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();
}

// Development OpenAPI endpoint
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map controller routes such as /api/auth/login
app.MapControllers();

app.Run();