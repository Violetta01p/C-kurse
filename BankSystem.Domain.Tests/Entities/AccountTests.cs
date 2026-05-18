using BankSystem.Domain.Entities;
using BankSystem.Domain.Exceptions;
using BankSystem.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BankSystem.Domain.Tests.Entities;

public class AccountTests
{
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public void CreateAccount_ShouldHaveZeroBalance()
    {
        // Act
        var account = new Account(Guid.NewGuid(), _userId);

        // Assert
        account.Balance.Amount.Should().Be(0);
        account.Balance.Currency.Should().Be("USD");
        account.UserId.Should().Be(_userId);
    }

    [Fact]
    public void Deposit_ShouldIncreaseBalanceAndRecordTransaction()
    {
        // Arrange
        var account = new Account(Guid.NewGuid(), _userId);
        var depositAmount = new Money(100, "USD");

        // Act
        account.Deposit(depositAmount);

        // Assert
        account.Balance.Amount.Should().Be(100);
        account.Transactions.Should().ContainSingle(t => t.Type == TransactionType.Deposit && t.Amount.Amount == 100);
    }

    [Fact]
    public void Withdraw_WithSufficientFunds_ShouldDecreaseBalanceAndRecordTransaction()
    {
        // Arrange
        var account = new Account(Guid.NewGuid(), _userId);
        account.Deposit(new Money(100, "USD"));

        // Act
        account.Withdraw(new Money(40, "USD"));

        // Assert
        account.Balance.Amount.Should().Be(60);
        account.Transactions.Should().HaveCount(2);
        account.Transactions.Last().Type.Should().Be(TransactionType.Withdrawal);
    }

    [Fact]
    public void Withdraw_WithInsufficientFunds_ShouldThrowInsufficientFundsException()
    {
        // Arrange
        var account = new Account(Guid.NewGuid(), _userId);
        account.Deposit(new Money(50, "USD"));

        // Act
        Action act = () => account.Withdraw(new Money(100, "USD"));

        // Assert
        act.Should().Throw<InsufficientFundsException>();
    }

    [Fact]
    public void Deposit_WhenAccountIsBlocked_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var account = new Account(Guid.NewGuid(), _userId);
        account.Block();

        // Act
        Action act = () => account.Deposit(new Money(10, "USD"));

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
