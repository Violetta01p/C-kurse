using BankSystem.Domain.Entities;
namespace BankSystem.Application.DTOs;
public record RegisterUserRequest(string FullName, string Login, string Password, Role Role = Role.Client);
