using BankSystem.Application.DTOs;
using BankSystem.Domain.Repositories;
using BankSystem.Domain.ValueObjects;
using BankSystem.Domain.Entities;

namespace BankSystem.Application.UseCases;

public class AdjustAccountBalanceUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AdjustAccountBalanceUseCase(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task ExecuteAsync(Guid accountId, decimal newBalanceAmount, string currency, CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);
        if (account == null) throw new Exception("Account not found.");

        var oldBalance = account.Balance;
        account.SetBalance(new Money(newBalanceAmount, currency));
        await _accountRepository.UpdateAsync(account, cancellationToken);
        
        var tx = new Transaction(Guid.NewGuid(), accountId, account.Balance, TransactionType.Deposit, $"Manual adjustment from {oldBalance.Amount} to {newBalanceAmount}");
        await _transactionRepository.AddAsync(tx, cancellationToken);
    }
}
