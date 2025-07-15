using InventoryOrderSystem.Data;
using InventoryOrderSystem.Data.Context;
using InventoryOrderSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InventoryOrderSystem.Data.Models;
using InventoryOrderSystem.ConsoleApp.Menus;

class Program
{
    public async static Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<AppDbContextFactory>()
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
        using var context = new AppDbContext(optionsBuilder.Options);

        await DataSeeder.SeedAsync(context);

        var userRepo = new UserRepository(context);
        User user = await LoginMenu.ShowAsync(userRepo);

        Console.WriteLine($"Hello, {user.Username}");

        if (user.Role == "Admin")
        {
            await AdminMenu.AdminMenuAsync(user, userRepo);
        }
        else if (user.Role == "Customer")
        {
            await CustomerMenu.CustomerMenuAsync(user, new ProductRepository(context), new OrderRepository(context));
        }
    }

    public static string ReadPassword()
    {
        var password = new System.Text.StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password.ToString();
    }
}