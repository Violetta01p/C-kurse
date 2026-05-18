// ============================================================
// Program.cs — Точка входу .NET застосунку (Composition Root)
// Тут налаштовуються всі залежності (Dependency Injection),
// безпека (JWT авторизація) та запускається HTTP сервер.
// Також ініціалізуються тестові дані (seed data) для демонстрації.
// ============================================================
using System.Text;
using BankSystem.Application.Interfaces;
using BankSystem.Application.UseCases;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;
using BankSystem.Domain.ValueObjects;
using BankSystem.Infrastructure.Auth;
using BankSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- Реєстрація сервісів ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger UI для тестування API

// Дозволяємо запити з фронтенду (React на localhost:5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// --- Налаштування JWT авторизації ---
// Цей ключ має збігатися з ключем у JwtTokenGenerator!
var key = Encoding.UTF8.GetBytes("super-secret-key-that-is-at-least-32-characters-long!");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,          // Токен протерміновується через 2 години
            ValidateIssuerSigningKey = true,
            ValidIssuer = "BankSystem",
            ValidAudience = "BankSystem",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// --- Реєстрація репозиторіїв (In-Memory замість реальної БД) ---
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
builder.Services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// --- Реєстрація Use Cases (бізнес-логіка) ---
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<AuthenticateUserUseCase>();
builder.Services.AddScoped<TransferMoneyUseCase>();
builder.Services.AddScoped<DepositUseCase>();
builder.Services.AddScoped<WithdrawUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetAllTransactionsUseCase>();
builder.Services.AddScoped<GetAccountTransactionsUseCase>();
builder.Services.AddScoped<BlockUserUseCase>();
builder.Services.AddScoped<AdjustAccountBalanceUseCase>();

var app = builder.Build();

// --- Ініціалізація тестових даних (seed data) ---
// У реальному проєкті це робиться через міграції бази даних
var userRepo = app.Services.GetRequiredService<IUserRepository>() as InMemoryUserRepository;
var accountRepo = app.Services.GetRequiredService<IAccountRepository>() as InMemoryAccountRepository;

// Тестові користувачі: адмін, менеджер та два клієнти
var adminUser = new User(Guid.NewGuid(), "Admin Adminov", "admin", "admin", Role.Admin);
var managerUser = new User(Guid.NewGuid(), "Manager Managerov", "manager", "manager", Role.Manager);
var clientUser1 = new User(Guid.NewGuid(), "Client One", "client1", "client1", Role.Client);
var clientUser2 = new User(Guid.NewGuid(), "Client Two", "client2", "client2", Role.Client);

userRepo!.Seed(adminUser);
userRepo.Seed(managerUser);
userRepo.Seed(clientUser1);
userRepo.Seed(clientUser2);

// Рахунки для клієнтів (client1 починає з $1000, client2 — з $0)
var account1 = new Account(Guid.Parse("11111111-1111-1111-1111-111111111111"), clientUser1.Id);
account1.Deposit(new Money(1000, "USD"));
var account2 = new Account(Guid.Parse("22222222-2222-2222-2222-222222222222"), clientUser2.Id);

accountRepo!.Seed(account1);
accountRepo.Seed(account2);

// --- Підключення middleware ---
app.UseSwagger();
app.UseSwaggerUI(); // Доступно на http://localhost:5000/swagger

app.UseCors("AllowAll");
app.UseAuthentication(); // Читає та перевіряє JWT токен
app.UseAuthorization();  // Перевіряє права доступу до ендпоінтів

app.MapControllers();

app.Run("http://localhost:5000");
