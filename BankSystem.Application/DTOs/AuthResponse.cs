namespace BankSystem.Application.DTOs;
public record AuthResponse(string Token, Guid UserId, string Role, Guid? AccountId);
