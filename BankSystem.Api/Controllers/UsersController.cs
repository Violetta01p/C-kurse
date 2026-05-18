// ============================================================
// UsersController.cs — API контролер для управління користувачами
// Доступний ТІЛЬКИ для ролей Manager та Admin.
// Надає можливість переглядати список всіх клієнтів та блокувати їх.
// ============================================================
using BankSystem.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager,Admin")] // Тільки для менеджерів та адміністраторів
public class UsersController : ControllerBase
{
    private readonly GetAllUsersUseCase _getAllUsersUseCase;
    private readonly BlockUserUseCase _blockUserUseCase;

    public UsersController(GetAllUsersUseCase getAllUsersUseCase, BlockUserUseCase blockUserUseCase)
    {
        _getAllUsersUseCase = getAllUsersUseCase;
        _blockUserUseCase = blockUserUseCase;
    }

    /// <summary>Отримати список усіх користувачів банку (CRM).</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _getAllUsersUseCase.ExecuteAsync();
        // Повертаємо тільки необхідні поля (без хешу пароля!)
        return Ok(users.Select(u => new { u.Id, u.FullName, u.Login, u.Role, u.IsBlocked }));
    }

    /// <summary>Заблокувати користувача та його рахунок.</summary>
    [HttpPost("{id}/block")]
    public async Task<IActionResult> Block(Guid id)
    {
        try
        {
            await _blockUserUseCase.ExecuteAsync(id);
            return Ok(new { Message = "User blocked successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
