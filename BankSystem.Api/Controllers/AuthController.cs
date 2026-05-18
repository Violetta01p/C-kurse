// ============================================================
// AuthController.cs — API контролер для авторизації
// Надає два відкритих (без авторизації) ендпоінти:
//   POST /api/auth/login    — вхід у систему, повертає JWT токен
//   POST /api/auth/register — реєстрація нового клієнта
// ============================================================
using BankSystem.Application.DTOs;
using BankSystem.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticateUserUseCase _authUseCase;
    private readonly RegisterUserUseCase _registerUseCase;

    public AuthController(AuthenticateUserUseCase authUseCase, RegisterUserUseCase registerUseCase)
    {
        _authUseCase = authUseCase;
        _registerUseCase = registerUseCase;
    }

    /// <summary>Вхід у систему. Повертає JWT токен при успішній автентифікації.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        try
        {
            var response = await _authUseCase.ExecuteAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>Реєстрація нового клієнта. Автоматично створює банківський рахунок.</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        try
        {
            var userId = await _registerUseCase.ExecuteAsync(request);
            return Ok(new { UserId = userId, Message = "Registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
