namespace BankSystem.Application.DTOs;

public record TransferMoneyRequest(Guid FromAccountId, Guid ToAccountId, decimal Amount, string Currency);
