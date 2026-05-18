using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;

namespace BankSystem.Application.UseCases;

public class GetAccountTransactionsUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    public GetAccountTransactionsUseCase(ITransactionRepository transactionRepository) => _transactionRepository = transactionRepository;
    
    public async Task<IEnumerable<Transaction>> ExecuteAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _transactionRepository.GetByAccountIdAsync(accountId, cancellationToken);
    }
}
