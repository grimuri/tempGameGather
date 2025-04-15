using FluentAssertions;
using GameGather.Domain.Aggregates.Users.ValueObjects;
using GameGather.Domain.UnitTests.TestUtils.Constants;

namespace GameGather.Domain.UnitTests.Aggregates.Users.ValueObjects;

public class PasswordTests
{
    [Fact]
    public void Create_Should_ReturnNewPassword_WhenValidValueIsProvided()
    {
        // Arrange
        var value = Constants.Password.Value;

        // Act
        var password = Password.Create(value);

        // Assert
        password.Value.Should().Be(value);
        password.LastModifiedOnUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
    
    [Fact]
    public void Load_Should_ReturnLoadedPassword_WhenValidValuesAreProvided()
    {
        // Arrange
        var value = Constants.Password.Value;
        var lastModifiedOnUtc = Constants.Password.LastModifiedOnUtc;
        var password = Password.Create(value);
        
        // Act
        password.Load(value, lastModifiedOnUtc);

        // Assert
        password.Value.Should().Be(value);
        password.LastModifiedOnUtc.Should().Be(lastModifiedOnUtc);
    }

    [Fact]
    public void IsExpired_Should_ReturnTrue_WhenPasswordIsExpired()
    {
        // Arrange
        var password = Password.Create(Constants.Password.Value);
        var lastModifiedOnUtc = DateTime.UtcNow.AddDays(-31);
        password.Load(Constants.Password.Value, lastModifiedOnUtc);
        
        // Act
        var isExpired = password.IsExpired(30);
        
        // Assert
        isExpired.Should().BeTrue();
    }

    [Fact]
    public void IsExpired_Should_ReturnFalse_WhenPasswordIsNotExpired()
    {
        // Arrange
        var password = Password.Create(Constants.Password.Value);
        var lastModifiedOnUtc = DateTime.UtcNow.AddDays(-10);
        password.Load(Constants.Password.Value, lastModifiedOnUtc);
        
        // Act
        var isExpired = password.IsExpired(30);
        
        // Assert
        isExpired.Should().BeFalse();
    }
}