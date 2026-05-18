using BankSystem.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BankSystem.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void CreateUser_ShouldInitializeProperly()
    {
        var user = new User(Guid.NewGuid(), "John Doe", "johndoe", "hash", Role.Client);
        
        user.FullName.Should().Be("John Doe");
        user.IsBlocked.Should().BeFalse();
        user.Role.Should().Be(Role.Client);
    }

    [Fact]
    public void BlockUser_ShouldSetIsBlockedToTrue()
    {
        var user = new User(Guid.NewGuid(), "John Doe", "johndoe", "hash", Role.Client);
        
        user.Block();
        
        user.IsBlocked.Should().BeTrue();
    }
}
