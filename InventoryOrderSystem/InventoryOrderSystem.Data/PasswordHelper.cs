using System.Security.Cryptography;
using System.Text;

namespace InventoryOrderSystem.Data;
public static class PasswordHelper
{
    public static (string Hash, string Salt) HashPassword(string password)
    {
        // Random salt
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        string salt = Convert.ToBase64String(saltBytes);

        string hash = HashWithSalt(password, salt);
        return (hash, salt);
    }

    public static string HashWithSalt(string password, string salt)
    {
        var combined = password + salt;
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(hashBytes);
        }
    }
}