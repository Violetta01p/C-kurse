// ============================================================
// JwtTokenGenerator.cs — Генератор JWT токенів
// JWT (JSON Web Token) — стандарт для безпечної передачі даних.
// Токен містить: ID користувача, ім'я та роль.
// Підписується секретним ключем (HMAC SHA256).
// Фронтенд надсилає токен у заголовку: Authorization: Bearer <token>
// ============================================================
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BankSystem.Infrastructure.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    // Секретний ключ для підпису токена (має бути мінімум 32 символи)
    // У продакшені цей ключ має зберігатися у секретах (не в коді!)
    private const string SecretKey = "super-secret-key-that-is-at-least-32-characters-long!";

    public string GenerateToken(User user)
    {
        // Додаємо до токена інформацію про користувача (claims — "претензії")
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ID
            new Claim(ClaimTypes.Name, user.FullName),                // Ім'я
            new Claim(ClaimTypes.Role, user.Role.ToString())          // Роль
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Токен дійсний 2 години
        var token = new JwtSecurityToken(
            issuer: "BankSystem",
            audience: "BankSystem",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        // Серіалізуємо токен у рядок (3 частини, розділені крапками)
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
