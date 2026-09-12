using Microsoft.AspNetCore.Mvc;
using PurchaseBill.Api.DTOs.PurchaseBills;
using PurchaseBill.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PurchaseBill.Api.Controllers;

[Authorize]

[ApiController]
[Route("api/purchase-bills")]
public class PurchaseBillsController : ControllerBase
{
    private readonly IPurchaseBillService _purchaseBillService;

    public PurchaseBillsController(
        IPurchaseBillService purchaseBillService)
    {
        _purchaseBillService = purchaseBillService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bills =
            await _purchaseBillService.GetAllAsync();

        return Ok(bills);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var bill =
            await _purchaseBillService.GetByIdAsync(id);

        if (bill is null)
        {
            return NotFound(new
            {
                message = "Purchase bill not found."
            });
        }

        return Ok(bill);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePurchaseBillDto request)
    {
        try
        {
            var bill =
                await _purchaseBillService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = bill.Id },
                bill
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}