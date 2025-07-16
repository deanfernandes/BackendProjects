using ProductReviews.Context;
using ProductReviews.Models;
using ProductReviews.Repositories;

class Program
{
    private static IUserRepository? _userRepo;
    private static IProductRepository? _productRepo;
    private static IReviewRepository? _reviewRepo;
    private static User? _loggedInUser;

    static async Task Main()
    {
        var context = new MongoDbContext();
        _userRepo = new UserRepository(context);
        _productRepo = new ProductRepository(context);
        _reviewRepo = new ReviewRepository(context);

        await SeedAdminUserAsync(_userRepo);
        await SeedProductsAsync(_productRepo);

        Console.WriteLine("Welcome! Please login.");

        while (_loggedInUser == null)
        {
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            var password = Console.ReadLine() ?? "";

            _loggedInUser = await _userRepo.GetByEmailAndPasswordAsync(email, password);
            if (_loggedInUser == null)
                Console.WriteLine("Invalid credentials, try again.");
        }

        Console.WriteLine($"Welcome, {_loggedInUser.Username} ({_loggedInUser.Role})!");

        if (_loggedInUser.Role == UserRole.Admin)
            await AdminMenuAsync();
        else
            await CustomerMenuAsync();
    }

    private static async Task ListUsersAsync()
    {
        var users = await _userRepo!.GetAllAsync();
        Console.WriteLine("\nUsers:");
        foreach (var user in users)
        {
            Console.WriteLine($"- {user.Id}: {user.Username} ({user.Email})");
        }
    }

    private static async Task AddUserAsync()
    {
        Console.Write("Enter username: ");
        var username = Console.ReadLine() ?? "";

        Console.Write("Enter email: ");
        var email = Console.ReadLine() ?? "";

        Console.Write("Enter password: ");
        var password = ReadPassword() ?? "";

        UserRole role = UserRole.Customer;

        if (_loggedInUser?.Role == UserRole.Admin)
        {
            Console.Write("Enter role (admin/customer): ");
            var roleInput = Console.ReadLine()?.Trim().ToLower();

            if (roleInput == "admin")
                role = UserRole.Admin;
        }

        var newUser = new User
        {
            Username = username,
            Email = email,
            Password = password,
            Role = role
        };

        await _userRepo!.CreateAsync(newUser);
        Console.WriteLine("User created!");
    }

    private static async Task UpdateUserAsync()
    {
        Console.Write("Enter user Id to update: ");
        var id = Console.ReadLine() ?? "";

        var user = await _userRepo!.GetByIdAsync(id);
        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.Write($"Enter new username (current: {user.Username}): ");
        var newUsername = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newUsername))
            user.Username = newUsername;

        Console.Write($"Enter new email (current: {user.Email}): ");
        var newEmail = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newEmail))
            user.Email = newEmail;

        Console.Write($"Enter new password (leave empty to keep current): ");
        var newPassword = ReadPassword();
        if (!string.IsNullOrWhiteSpace(newPassword))
            user.Password = newPassword;

        await _userRepo.UpdateAsync(user);
        Console.WriteLine("User updated!");
    }


    private static async Task DeleteUserAsync()
    {
        Console.Write("Enter user Id to delete: ");
        var id = Console.ReadLine() ?? "";

        var user = await _userRepo!.GetByIdAsync(id);
        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        await _userRepo.DeleteAsync(id);
        Console.WriteLine("User deleted!");
    }

    public static string ReadPassword()
    {
        var pass = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                pass += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
            {
                pass = pass[0..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return pass;
    }

    private static async Task AdminMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- Admin Main Menu ---");
            Console.WriteLine("1. Manage users");
            Console.WriteLine("2. Manage products");
            Console.WriteLine("3. Logout");
            Console.Write("Choose option: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await ManageUsersMenuAsync();
                    break;
                case "2":
                    await ProductCrudMenuAsync();
                    break;
                case "3":
                    _loggedInUser = null;
                    Console.WriteLine("Logged out.");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static async Task ManageUsersMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- User Management ---");
            Console.WriteLine("1. List all users");
            Console.WriteLine("2. Add new user");
            Console.WriteLine("3. Update user");
            Console.WriteLine("4. Delete user");
            Console.WriteLine("5. Back to admin menu");
            Console.Write("Choose option: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await ListUsersAsync();
                    break;
                case "2":
                    await AddUserAsync();
                    break;
                case "3":
                    await UpdateUserAsync();
                    break;
                case "4":
                    await DeleteUserAsync();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static async Task ProductCrudMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- Product Management ---");
            Console.WriteLine("1. List all products");
            Console.WriteLine("2. Add product");
            Console.WriteLine("3. Update product");
            Console.WriteLine("4. Delete product");
            Console.WriteLine("5. Back to admin menu");
            Console.Write("Choose option: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    var products = await _productRepo.GetAllAsync();
                    foreach (var p in products)
                        Console.WriteLine($"- {p.Id}: {p.Name} | {p.Price:C} | {p.Description}");
                    break;

                case "2":
                    Console.Write("Product name: ");
                    var name = Console.ReadLine() ?? "";
                    Console.Write("Description: ");
                    var desc = Console.ReadLine() ?? "";
                    Console.Write("Price: ");
                    decimal.TryParse(Console.ReadLine(), out var price);
                    await _productRepo.CreateAsync(new Product { Name = name, Description = desc, Price = price });
                    Console.WriteLine("Product added.");
                    break;

                case "3":
                    Console.Write("Enter product ID to update: ");
                    var updateId = Console.ReadLine() ?? "";
                    var product = await _productRepo.GetByIdAsync(updateId);
                    if (product == null)
                    {
                        Console.WriteLine("Product not found.");
                        break;
                    }
                    Console.Write($"New name (current: {product.Name}): ");
                    var newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                        product.Name = newName;

                    Console.Write($"New description (current: {product.Description}): ");
                    var newDesc = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newDesc))
                        product.Description = newDesc;

                    Console.Write($"New price (current: {product.Price}): ");
                    var priceStr = Console.ReadLine();
                    if (decimal.TryParse(priceStr, out var newPrice))
                        product.Price = newPrice;

                    await _productRepo.UpdateAsync(product);
                    Console.WriteLine("Product updated.");
                    break;

                case "4":
                    Console.Write("Enter product ID to delete: ");
                    var delId = Console.ReadLine() ?? "";
                    await _productRepo.DeleteAsync(delId);
                    Console.WriteLine("Product deleted.");
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static async Task CustomerMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- Customer Menu ---");
            Console.WriteLine("1. Write a product review");
            Console.WriteLine("2. Logout");
            Console.Write("Choose option: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await CreateReviewAsync();
                    break;
                case "2":
                    _loggedInUser = null;
                    Console.WriteLine("Logged out.");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static async Task CreateReviewAsync()
    {
        var products = await _productRepo.GetAllAsync();
        if (products.Count == 0)
        {
            Console.WriteLine("No products available to review.");
            return;
        }

        Console.WriteLine("\nAvailable products:");
        foreach (var p in products)
            Console.WriteLine($"- {p.Id}: {p.Name} ({p.Price:C})");

        Console.Write("Enter product ID to review: ");
        var productId = Console.ReadLine() ?? "";

        var product = products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        int rating = -1;
        while (rating < 0 || rating > 5)
        {
            Console.Write("Enter rating (0 to 5): ");
            int.TryParse(Console.ReadLine(), out rating);
        }

        Console.Write("Enter optional comment: ");
        var comment = Console.ReadLine() ?? "";

        var review = new Review
        {
            ProductId = product.Id,
            UserId = _loggedInUser!.Id,
            Rating = rating,
            Comment = comment
        };

        await _reviewRepo.CreateAsync(review);
        Console.WriteLine("Thank you! Review submitted.");
    }

    private static async Task SeedAdminUserAsync(IUserRepository userRepo)
    {
        var existingAdmin = await userRepo.GetByEmailAndPasswordAsync("admin", "admin");
        if (existingAdmin != null)
        {
            Console.WriteLine("Admin user already exists.");
            return;
        }

        var adminUser = new User
        {
            Username = "admin",
            Email = "admin",
            Password = "admin",
            Role = UserRole.Admin
        };

        await userRepo.CreateAsync(adminUser);
        Console.WriteLine("Seeded admin user (email: admin, password: admin).");
    }

    private static async Task SeedProductsAsync(IProductRepository productRepo)
    {
        var existingProducts = await productRepo.GetAllAsync();
        if (existingProducts.Any())
        {
            Console.WriteLine("Products already exist.");
            return;
        }

        var sampleProducts = new List<Product>
    {
        new Product { Name = "Laptop", Description = "High-performance laptop", Price = 999.99m },
        new Product { Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99m },
        new Product { Name = "Headphones", Description = "Noise-cancelling headphones", Price = 149.99m }
    };

        foreach (var product in sampleProducts)
        {
            await productRepo.CreateAsync(product);
        }

        Console.WriteLine("Sample products seeded.");
    }
}
