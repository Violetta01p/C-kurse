namespace BankSystem.Application.DTOs;
public record DepositWithdrawRequest(Guid AccountId, decimal Amount, string Currency);
