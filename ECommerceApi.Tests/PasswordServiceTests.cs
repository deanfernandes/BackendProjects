using ECommerceApi.Services;
using Xunit;
using Microsoft.AspNetCore.Identity;

public class PasswordServiceTests
{
    private readonly PasswordService _passwordService = new();

    private class DummyUser { }

    [Fact]
    public void HashPassword_ReturnsNonEmptyHash()
    {
        var user = new DummyUser();
        var password = "MySecurePassword123!";

        var hashedPassword = _passwordService.HashPassword(user, password);

        Assert.False(string.IsNullOrWhiteSpace(hashedPassword));
    }

    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsSuccess()
    {
        var user = new DummyUser();
        var password = "MySecurePassword123!";
        var hashedPassword = _passwordService.HashPassword(user, password);

        var result = _passwordService.VerifyPassword(user, hashedPassword, password);

        Assert.Equal(PasswordVerificationResult.Success, result);
    }

    [Fact]
    public void VerifyPassword_ReturnsFailed_ForIncorrectPassword()
    {
        var user = new DummyUser();
        var correctPassword = "MySecurePassword123!";
        var wrongPassword = "WrongPassword!";
        var hashedPassword = _passwordService.HashPassword(user, correctPassword);

        var result = _passwordService.VerifyPassword(user, hashedPassword, wrongPassword);

        Assert.Equal(PasswordVerificationResult.Failed, result);
    }
}