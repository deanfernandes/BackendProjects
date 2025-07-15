using InventoryOrderSystem.Data.Context;
using InventoryOrderSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryOrderSystem.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            Console.WriteLine("Database already seeded.");
            return;
        }

        var (hash, salt) = PasswordHelper.HashPassword("admin");
        var admin = new User
        {
            Username = "admin",
            PasswordHash = hash,
            Salt = salt,
            Role = "Admin",
            Email = "admin@gmail.com",
        };

        (hash, salt) = PasswordHelper.HashPassword("password");
        var customer = new User
        {
            Username = "customer",
            PasswordHash = hash,
            Salt = salt,
            Role = "Customer",
            Email = "customer@gmail.com",
        };

        var products = new List<Product>
        {
            new() { Name = "Keyboard", Price = 29.99M, Stock = 10 },
            new() { Name = "Mouse", Price = 19.99M, Stock = 20 },
            new() { Name = "Monitor", Price = 129.99M, Stock = 5 }
        };

        context.Users.AddRange(admin, customer);
        context.Products.AddRange(products);

        await context.SaveChangesAsync();
    }
}
