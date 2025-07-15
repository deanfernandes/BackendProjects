using InventoryOrderSystem.Data.Models;
using InventoryOrderSystem.Data.Repositories;

namespace InventoryOrderSystem.ConsoleApp.Menus
{
    public static class AdminMenu
    {
        public static async Task AdminMenuAsync(User admin, IUserRepository userRepo)
        {
            while (true)
            {
                Console.WriteLine("\n=== Admin Menu ===");
                Console.WriteLine("1. List Users");
                Console.WriteLine("2. Add User");
                Console.WriteLine("0. Logout");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ListUsersAsync(userRepo);
                        break;
                    case "2":
                        await AddUserAsync(userRepo);
                        break;
                    case "0":
                        Console.WriteLine("Logging out...");
                        return; // Exit
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static async Task ListUsersAsync(IUserRepository userRepo)
        {
            var users = await userRepo.GetAllAsync();
            Console.WriteLine("\n--- Users ---");
            foreach (var u in users)
            {
                Console.WriteLine($"Id: {u.Id}, Username: {u.Username}, Role: {u.Role}");
            }
        }

        static async Task AddUserAsync(IUserRepository userRepo)
        {
            Console.Write("New Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Program.ReadPassword();

            Console.Write("Role (Admin/Customer): ");
            string role = Console.ReadLine() ?? "Customer";

            var newUser = new User
            {
                Username = username,
                PasswordHash = password, // Hash will be done later in repo AddAsync()
                Role = role
            };

            await userRepo.AddAsync(newUser);
            Console.WriteLine("User added successfully.");
        }
    }
}
