using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Services;

public interface IPasswordService
{
    string HashPassword<T>(T user, string password);
    PasswordVerificationResult VerifyPassword<T>(T user, string hashedPassword, string providedPassword);
}