using BankSystem.Application.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;

namespace BankSystem.Application.UseCases;

public class RegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;

    public RegisterUserUseCase(IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<Guid> ExecuteAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
        if (existingUser != null) throw new Exception("User with this login already exists.");

        var user = new User(Guid.NewGuid(), request.FullName, request.Login, request.Password, request.Role);
        await _userRepository.AddAsync(user, cancellationToken);
        
        if (request.Role == Role.Client)
        {
             var account = new Account(Guid.NewGuid(), user.Id);
             await _accountRepository.UpdateAsync(account, cancellationToken);
        }
        
        return user.Id;
    }
}
