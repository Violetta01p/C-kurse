// ============================================================
// Transaction.cs — Сутність "Транзакція"
// Записується автоматично при кожній операції з рахунком.
// Є незмінною (immutable) — після створення не змінюється.
// AccountId прив'язує транзакцію до конкретного рахунку.
// ============================================================
using BankSystem.Domain.ValueObjects;

namespace BankSystem.Domain.Entities;

public class Transaction
{
    // Унікальний ідентифікатор транзакції
    public Guid Id { get; private set; }

    // Рахунок, до якого належить ця транзакція
    public Guid AccountId { get; private set; }

    // Сума операції (завжди позитивна; тип операції вказує напрямок)
    public Money Amount { get; private set; }

    // Тип: Deposit (поповнення), Withdrawal (зняття), Transfer (переказ)
    public TransactionType Type { get; private set; }

    // Дата та час операції (UTC)
    public DateTime Date { get; private set; }

    // Текстовий опис операції (наприклад, "Transfer to 22222222-...")
    public string Description { get; private set; }

    public Transaction(Guid id, Guid accountId, Money amount, TransactionType type, string description, DateTime? date = null)
    {
        Id = id;
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Description = description;
        Date = date ?? DateTime.UtcNow; // Якщо дата не передана — беремо поточний час
    }
}
