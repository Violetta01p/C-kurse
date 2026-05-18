using BankSystem.Application.DTOs;
using BankSystem.Application.UseCases;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Exceptions;
using BankSystem.Domain.Repositories;
using BankSystem.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankSystem.Application.Tests.UseCases;

public class TransferMoneyUseCaseTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly TransferMoneyUseCase _useCase;
    private readonly Guid _userId = Guid.NewGuid();

    public TransferMoneyUseCaseTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _useCase = new TransferMoneyUseCase(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldTransferMoney_WhenFundsAreSufficient()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        
        var fromAccount = new Account(fromAccountId, _userId);
        fromAccount.Deposit(new Money(100, "USD"));
        
        var toAccount = new Account(toAccountId, Guid.NewGuid());

        _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(fromAccountId, default))
                              .ReturnsAsync(fromAccount);
        _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(toAccountId, default))
                              .ReturnsAsync(toAccount);

        var request = new TransferMoneyRequest(fromAccountId, toAccountId, 50, "USD");

        // Act
        await _useCase.ExecuteAsync(request);

        // Assert
        fromAccount.Balance.Amount.Should().Be(50);
        toAccount.Balance.Amount.Should().Be(50);
        
        fromAccount.Transactions.Last().Type.Should().Be(TransactionType.Transfer);
        toAccount.Transactions.Last().Type.Should().Be(TransactionType.Transfer);
        
        _accountRepositoryMock.Verify(repo => repo.UpdateAsync(fromAccount, default), Times.Once);
        _accountRepositoryMock.Verify(repo => repo.UpdateAsync(toAccount, default), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowException_WhenFromAccountNotFound()
    {
        // Arrange
        var request = new TransferMoneyRequest(Guid.NewGuid(), Guid.NewGuid(), 50, "USD");
        _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(request.FromAccountId, default))
                              .ReturnsAsync((Account?)null);

        // Act
        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Account not found: *");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInsufficientFundsException_WhenFundsAreNotSufficient()
    {
        // Arrange
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();
        
        var fromAccount = new Account(fromAccountId, _userId);
        fromAccount.Deposit(new Money(10, "USD"));
        
        var toAccount = new Account(toAccountId, Guid.NewGuid());

        _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(fromAccountId, default))
                              .ReturnsAsync(fromAccount);
        _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(toAccountId, default))
                              .ReturnsAsync(toAccount);

        var request = new TransferMoneyRequest(fromAccountId, toAccountId, 50, "USD");

        // Act
        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<InsufficientFundsException>();
    }
}
