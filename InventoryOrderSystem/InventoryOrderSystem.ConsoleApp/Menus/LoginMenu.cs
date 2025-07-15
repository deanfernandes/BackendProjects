using InventoryOrderSystem.Data.Models;
using InventoryOrderSystem.Data.Repositories;

namespace InventoryOrderSystem.ConsoleApp.Menus
{
    public static class LoginMenu
    {
        public static async Task<User> ShowAsync(IUserRepository userRepo)
        {
            User? user = null;

            while (user == null)
            {
                Console.WriteLine("\nLogin to Inventory Order System");
                Console.Write("Username: ");
                string username = Console.ReadLine() ?? "";

                Console.Write("Password: ");
                string password = Program.ReadPassword();

                user = await userRepo.AuthenticateAsync(username, password);

                if (user == null)
                {
                    Console.WriteLine("Invalid username or password. Try again.");
                }
            }

            return user;
        }
    }
}