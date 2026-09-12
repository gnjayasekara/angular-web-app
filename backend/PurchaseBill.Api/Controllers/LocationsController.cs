using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace PurchaseBill.Api.Controllers;

[Authorize]

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public LocationsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _dbContext.LocationDetails
            .OrderBy(x => x.LocationName)
            .Select(x => new
            {
                x.LocationCode,
                x.LocationName
            })
            .ToListAsync();

        return Ok(locations);
    }
}