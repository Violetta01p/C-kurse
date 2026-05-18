// ============================================================
// Account.cs — Агрегат "Банківський рахунок" (Aggregate Root)
// У DDD Aggregate Root — головна сутність, яка захищає бізнес-правила.
// Account контролює:
//   - баланс (можна тільки збільшити через Deposit або зменшити через Withdraw)
//   - статус блокування (заблокований рахунок не може проводити операції)
//   - власника рахунку через UserId
//   - власну історію транзакцій (список Transaction)
// ============================================================
using BankSystem.Domain.Exceptions;
using BankSystem.Domain.ValueObjects;

namespace BankSystem.Domain.Entities;

public class Account
{
    // Унікальний ідентифікатор рахунку
    public Guid Id { get; private set; }

    // Власник рахунку (посилання на User)
    public Guid UserId { get; private set; }

    // Поточний баланс (об'єкт Money з сумою та валютою)
    public Money Balance { get; private set; }

    // Чи заблокований рахунок (менеджер може заблокувати)
    public bool IsBlocked { get; private set; }

    // Приватний список транзакцій (зовні доступний лише для читання)
    private readonly List<Transaction> _transactions = new();
    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

    public Account(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
        Balance = new Money(0, "USD"); // Новий рахунок завжди починається з нуля
        IsBlocked = false;
    }

    // Метод для ручного встановлення балансу (використовується менеджером)
    public void SetBalance(Money amount)
    {
        Balance = amount;
    }

    // Заблокувати рахунок (наприклад, через підозрілу активність)
    public void Block()
    {
        IsBlocked = true;
    }

    // Розблокувати рахунок
    public void Unblock()
    {
        IsBlocked = false;
    }

    // Внутрішня перевірка: операція неможлива, якщо рахунок заблоковано
    private void EnsureNotBlocked()
    {
        if (IsBlocked) throw new InvalidOperationException("Account is blocked.");
    }

    // Поповнення рахунку (Deposit): збільшує баланс і записує транзакцію
    public void Deposit(Money amount, string description = "Deposit", TransactionType type = TransactionType.Deposit)
    {
        EnsureNotBlocked();
        Balance += amount;
        _transactions.Add(new Transaction(Guid.NewGuid(), Id, amount, type, description));
    }

    // Зняття з рахунку (Withdraw): перевіряє достатність коштів, зменшує баланс
    public void Withdraw(Money amount, string description = "Withdrawal", TransactionType type = TransactionType.Withdrawal)
    {
        EnsureNotBlocked();
        if (Balance.Amount < amount.Amount)
        {
            // Якщо грошей недостатньо — викидаємо доменне виключення
            throw new InsufficientFundsException("Insufficient funds to withdraw.");
        }

        Balance -= amount;
        _transactions.Add(new Transaction(Guid.NewGuid(), Id, amount, type, description));
    }
}
