using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;

namespace BankSystem.Application.UseCases;

public class GetAllTransactionsUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    public GetAllTransactionsUseCase(ITransactionRepository transactionRepository) => _transactionRepository = transactionRepository;
    
    public async Task<IEnumerable<Transaction>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await _transactionRepository.GetAllAsync(cancellationToken);
    }
}
