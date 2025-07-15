﻿using System.Security.Cryptography;
using System.Text;

namespace InventoryOrderSystem.Data;
public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
