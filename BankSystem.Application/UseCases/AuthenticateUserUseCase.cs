// ============================================================
// AuthenticateUserUseCase.cs — Use Case "Аутентифікація (Вхід у систему)"
// Перевіряє логін/пароль, і якщо все вірно — повертає JWT токен.
// JWT токен — це зашифрований рядок, який містить дані про користувача
// та його роль. Фронтенд зберігає його та надсилає з кожним запитом.
// ============================================================
using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Repositories;

namespace BankSystem.Application.UseCases;

public class AuthenticateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator; // Генератор JWT токенів

    public AuthenticateUserUseCase(IUserRepository userRepository, IAccountRepository accountRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> ExecuteAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        // Шукаємо користувача за логіном
        var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);

        // Якщо логін не знайдено або пароль невірний — відмовляємо у доступі
        if (user == null || user.PasswordHash != request.Password)
            throw new Exception("Invalid login or password.");

        // Заблокований користувач не може увійти в систему
        if (user.IsBlocked)
            throw new Exception("User is blocked.");

        // Генеруємо JWT токен з даними про роль та ID користувача
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Для клієнтів додатково повертаємо ID їхнього рахунку
        Guid? accountId = null;
        if (user.Role == Domain.Entities.Role.Client)
        {
            var account = await _accountRepository.GetByUserIdAsync(user.Id, cancellationToken);
            accountId = account?.Id;
        }

        return new AuthResponse(token, user.Id, user.Role.ToString(), accountId);
    }
}
