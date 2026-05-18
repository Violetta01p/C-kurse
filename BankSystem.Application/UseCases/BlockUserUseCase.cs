using BankSystem.Domain.Repositories;

namespace BankSystem.Application.UseCases;

public class BlockUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    
    public BlockUserUseCase(IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null) throw new Exception("User not found");
        
        user.Block();
        await _userRepository.UpdateAsync(user, cancellationToken);
        
        var account = await _accountRepository.GetByUserIdAsync(userId, cancellationToken);
        if (account != null)
        {
            account.Block();
            await _accountRepository.UpdateAsync(account, cancellationToken);
        }
    }
}
