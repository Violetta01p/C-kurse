// ============================================================
// TransferMoneyUseCase.cs — Use Case "Переказ коштів"
// У DDD Use Case (він же Application Service) — це оркестратор.
// Він сам не містить бізнес-логіки, а лише координує роботу
// доменних об'єктів (Account) через репозиторій.
// Алгоритм:
//   1. Знайти рахунок відправника
//   2. Знайти рахунок отримувача
//   3. Списати кошти з рахунку відправника (Account.Withdraw)
//   4. Зарахувати кошти на рахунок отримувача (Account.Deposit)
//   5. Зберегти обидва рахунки
// ============================================================
using BankSystem.Application.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;
using BankSystem.Domain.ValueObjects;

namespace BankSystem.Application.UseCases;

public class TransferMoneyUseCase
{
    // Репозиторій — посередник між бізнес-логікою і базою даних
    private readonly IAccountRepository _accountRepository;

    public TransferMoneyUseCase(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task ExecuteAsync(TransferMoneyRequest request, CancellationToken cancellationToken = default)
    {
        // Крок 1: завантажуємо обидва рахунки з репозиторію
        var fromAccount = await _accountRepository.GetByIdAsync(request.FromAccountId, cancellationToken);
        var toAccount = await _accountRepository.GetByIdAsync(request.ToAccountId, cancellationToken);

        if (fromAccount == null) throw new Exception("Account not found: " + request.FromAccountId);
        if (toAccount == null) throw new Exception("Account not found: " + request.ToAccountId);

        var transferAmount = new Money(request.Amount, request.Currency);

        // Крок 2: домен сам перевіряє достатність коштів та статус блокування
        fromAccount.Withdraw(transferAmount, $"Transfer to {toAccount.Id}", TransactionType.Transfer);
        toAccount.Deposit(transferAmount, $"Transfer from {fromAccount.Id}", TransactionType.Transfer);

        // Крок 3: зберігаємо оновлені рахунки
        await _accountRepository.UpdateAsync(fromAccount, cancellationToken);
        await _accountRepository.UpdateAsync(toAccount, cancellationToken);
    }
}
