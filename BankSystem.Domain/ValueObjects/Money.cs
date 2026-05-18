// ============================================================
// Money.cs — Value Object "Гроші"
// У DDD Value Object — це незмінний об'єкт, який описує значення.
// Money зберігає суму та валюту. Два об'єкти Money вважаються
// рівними, якщо в них однакова сума і валюта.
// Перевантаження операторів + та - дозволяє зручно складати/віднімати суми.
// ============================================================
namespace BankSystem.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    // Оператор додавання: 100 USD + 50 USD = 150 USD
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency) throw new InvalidOperationException("Currency mismatch.");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    // Оператор віднімання: 100 USD - 40 USD = 60 USD
    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency) throw new InvalidOperationException("Currency mismatch.");
        return new Money(a.Amount - b.Amount, a.Currency);
    }
}
