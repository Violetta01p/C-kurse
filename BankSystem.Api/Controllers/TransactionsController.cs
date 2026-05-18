using BankSystem.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager,Admin")]
public class TransactionsController : ControllerBase
{
    private readonly GetAllTransactionsUseCase _getAllUseCase;

    public TransactionsController(GetAllTransactionsUseCase getAllUseCase)
    {
        _getAllUseCase = getAllUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var txs = await _getAllUseCase.ExecuteAsync();
        return Ok(txs);
    }
}
