using System.Collections.Concurrent;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;

namespace BankSystem.Infrastructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly ConcurrentDictionary<Guid, Account> _accounts = new();

    public void Seed(Account account)
    {
        _accounts[account.Id] = account;
    }

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _accounts.TryGetValue(id, out var account);
        return Task.FromResult(account);
    }

    public Task<Account?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var account = _accounts.Values.FirstOrDefault(a => a.UserId == userId);
        return Task.FromResult(account);
    }

    public Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_accounts.Values.AsEnumerable());
    }

    public Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        _accounts[account.Id] = account;
        return Task.CompletedTask;
    }
}
