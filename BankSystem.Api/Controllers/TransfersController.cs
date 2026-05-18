using BankSystem.Application.DTOs;
using BankSystem.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransfersController : ControllerBase
{
    private readonly TransferMoneyUseCase _transferMoneyUseCase;

    public TransfersController(TransferMoneyUseCase transferMoneyUseCase)
    {
        _transferMoneyUseCase = transferMoneyUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] TransferMoneyRequest request)
    {
        try
        {
            await _transferMoneyUseCase.ExecuteAsync(request);
            return Ok(new { Message = "Transfer successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
