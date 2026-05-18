using BankSystem.Application.DTOs;
using BankSystem.Domain.Repositories;
using BankSystem.Domain.ValueObjects;

namespace BankSystem.Application.UseCases;

public class WithdrawUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public WithdrawUseCase(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task ExecuteAsync(DepositWithdrawRequest request, CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account == null) throw new Exception("Account not found.");

        account.Withdraw(new Money(request.Amount, request.Currency));
        
        await _accountRepository.UpdateAsync(account, cancellationToken);
        var latestTx = account.Transactions.Last();
        await _transactionRepository.AddAsync(latestTx, cancellationToken);
    }
}
