using BankSystem.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BankSystem.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Add_MoneysWithSameCurrency_ShouldReturnSum()
    {
        // Arrange
        var m1 = new Money(100, "USD");
        var m2 = new Money(50, "USD");

        // Act
        var result = m1 + m2;

        // Assert
        result.Amount.Should().Be(150);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Add_MoneysWithDifferentCurrency_ShouldThrowException()
    {
        // Arrange
        var m1 = new Money(100, "USD");
        var m2 = new Money(50, "EUR");

        // Act
        Action act = () => { var res = m1 + m2; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cannot add moneys with different currencies");
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrowArgumentException()
    {
        // Act
        Action act = () => new Money(-10, "USD");

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("Amount cannot be negative*");
    }
}
