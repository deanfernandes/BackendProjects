using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string HashPassword<T>(T user, string password)
    {
        return _passwordHasher.HashPassword(user!, password);
    }

    public PasswordVerificationResult VerifyPassword<T>(T user, string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user!, hashedPassword, providedPassword);
    }
}