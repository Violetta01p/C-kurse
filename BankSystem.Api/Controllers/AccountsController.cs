// ============================================================
// AccountsController.cs — API контролер для операцій з рахунком
// Всі ендпоінти потребують авторизації (JWT токен обов'язковий).
// Клієнт може бачити ТІЛЬКИ свій рахунок (перевіряємо UserId).
// Менеджер та Адмін мають доступ до всіх рахунків.
// ============================================================
using BankSystem.Application.DTOs;
using BankSystem.Application.UseCases;
using BankSystem.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Всі методи потребують авторизованого JWT токена
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly GetAccountTransactionsUseCase _transactionsUseCase;
    private readonly DepositUseCase _depositUseCase;
    private readonly WithdrawUseCase _withdrawUseCase;

    public AccountsController(
        IAccountRepository accountRepository,
        GetAccountTransactionsUseCase transactionsUseCase,
        DepositUseCase depositUseCase,
        WithdrawUseCase withdrawUseCase)
    {
        _accountRepository = accountRepository;
        _transactionsUseCase = transactionsUseCase;
        _depositUseCase = depositUseCase;
        _withdrawUseCase = withdrawUseCase;
    }

    /// <summary>Отримати баланс рахунку. Клієнт бачить тільки свій рахунок.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null) return NotFound();

        // Перевіряємо, що клієнт не намагається переглянути чужий рахунок
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role != "Admin" && role != "Manager" && account.UserId.ToString() != userId)
        {
            return Forbid(); // 403 Forbidden
        }

        return Ok(new
        {
            account.Id,
            Balance = account.Balance.Amount,
            Currency = account.Balance.Currency,
            account.IsBlocked
        });
    }

    /// <summary>Отримати список транзакцій для конкретного рахунку.</summary>
    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid id)
    {
        var txs = await _transactionsUseCase.ExecuteAsync(id);
        return Ok(txs);
    }

    /// <summary>Поповнити рахунок (Deposit).</summary>
    [HttpPost("{id}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] DepositWithdrawRequest request)
    {
        try
        {
            if (id != request.AccountId) return BadRequest();
            await _depositUseCase.ExecuteAsync(request);
            return Ok(new { Message = "Deposit successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>Зняти кошти з рахунку (Withdraw).</summary>
    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] DepositWithdrawRequest request)
    {
        try
        {
            if (id != request.AccountId) return BadRequest();
            await _withdrawUseCase.ExecuteAsync(request);
            return Ok(new { Message = "Withdraw successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
