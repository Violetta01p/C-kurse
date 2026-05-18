using System.Collections.Concurrent;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;

namespace BankSystem.Infrastructure.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly ConcurrentDictionary<Guid, Transaction> _transactions = new();

    public Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var txs = _transactions.Values.Where(t => t.AccountId == accountId).OrderByDescending(t => t.Date).AsEnumerable();
        return Task.FromResult(txs);
    }

    public Task<IEnumerable<Transaction>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_transactions.Values.OrderByDescending(t => t.Date).AsEnumerable());
    }

    public Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        _transactions[transaction.Id] = transaction;
        return Task.CompletedTask;
    }
}
