using BankSystem.Domain.Entities;
using BankSystem.Domain.Repositories;

namespace BankSystem.Application.UseCases;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;
    public GetAllUsersUseCase(IUserRepository userRepository) => _userRepository = userRepository;
    
    public async Task<IEnumerable<User>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }
}
