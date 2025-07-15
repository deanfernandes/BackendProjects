using InventoryOrderSystem.Data;
using InventoryOrderSystem.Data.Context;
using InventoryOrderSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InventoryOrderSystem.Data.Models;

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
        User? user = null;
        while (user == null)
        {
            Console.WriteLine("\nLogin to Inventory Order System");
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = ReadPassword();

            user = await userRepo.AuthenticateAsync(username, password);

            if (user == null)
            {
                Console.WriteLine("Invalid username or password. Try again.");
            }
        }

        Console.WriteLine($"Hello, {user.Username} ({user.Role})");

        if (user.Role == "Admin")
        {
            //TODO: admin menu
        }
        else if (user.Role == "Customer")
        {
            //TODO: customer menu
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