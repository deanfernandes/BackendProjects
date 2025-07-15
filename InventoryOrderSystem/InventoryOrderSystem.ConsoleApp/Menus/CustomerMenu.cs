using InventoryOrderSystem.Data.Models;
using InventoryOrderSystem.Data.Repositories;

namespace InventoryOrderSystem.ConsoleApp.Menus
{
    public static class CustomerMenu
    {
        public static async Task CustomerMenuAsync(User customer, IProductRepository productRepo, IOrderRepository orderRepo)
        {
            while (true)
            {
                Console.WriteLine("\n=== Customer Menu ===");
                Console.WriteLine("1. Place Order");
                Console.WriteLine("2. View Order History");
                Console.WriteLine("3. Log Out");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    await PlaceOrderAsync(customer, productRepo, orderRepo);
                }
                else if (choice == "2")
                {
                    await ViewOrderHistoryAsync(customer, orderRepo);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Logging out...");
                    break; // Exit
                }
                else
                {
                    Console.WriteLine("Invalid option, try again.");
                }
            }
        }

        static async Task PlaceOrderAsync(User customer, IProductRepository productRepo, IOrderRepository orderRepo)
        {
            var cart = new List<OrderItem>();

            while (true)
            {
                var products = (await productRepo.GetAllAsync()).ToList();

                Console.WriteLine("\nAvailable Products:");
                foreach (var p in products)
                    Console.WriteLine($"Id: {p.Id}, Name: {p.Name}, Price: {p.Price:C}, Stock: {p.Stock}");

                Console.Write("Enter product ID to add to order (0 to checkout, -1 to cancel): ");
                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Invalid input, try again.");
                    continue;
                }

                if (productId < 0)
                {
                    return;
                }

                if (productId == 0)
                {
                    if (cart.Count == 0)
                    {
                        Console.WriteLine("Your order is empty. Add some products before checkout.");
                        continue;
                    }

                    decimal total = 0;
                    Console.WriteLine("\nYour Order:");
                    foreach (var item in cart)
                    {
                        var product = products.First(p => p.Id == item.ProductId);
                        decimal itemTotal = product.Price * item.Quantity;
                        total += itemTotal;
                        Console.WriteLine($"{product.Name} x {item.Quantity} = {itemTotal:C}");
                    }
                    Console.WriteLine($"Total: {total:C}");

                    Console.Write("Confirm order? (Y/N): ");
                    var confirm = Console.ReadLine()?.Trim().ToUpper();
                    if (confirm == "Y")
                    {
                        foreach (var item in cart)
                        {
                            var product = products.First(p => p.Id == item.ProductId);
                            product.Stock -= item.Quantity;
                            await productRepo.UpdateAsync(product);
                        }

                        var order = new Order
                        {
                            UserId = customer.Id,
                            Items = cart
                        };

                        await orderRepo.AddAsync(order);
                        Console.WriteLine("Order placed successfully!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Order cancelled. Returning to product selection.");
                        continue;
                    }
                }

                var selectedProduct = products.FirstOrDefault(p => p.Id == productId);
                if (selectedProduct == null)
                {
                    Console.WriteLine("Product not found. Try again.");
                    continue;
                }

                Console.Write($"Enter quantity for {selectedProduct.Name} (stock available: {selectedProduct.Stock}): ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity. Try again.");
                    continue;
                }

                var existingCartItem = cart.FirstOrDefault(i => i.ProductId == productId);
                int totalRequested = quantity + (existingCartItem?.Quantity ?? 0);
                if (totalRequested > selectedProduct.Stock)
                {
                    Console.WriteLine("Quantity exceeds available stock. Try again.");
                    continue;
                }

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    cart.Add(new OrderItem { ProductId = productId, Quantity = quantity });
                }

                Console.WriteLine($"{quantity} x {selectedProduct.Name} added to your order.");
            }
        }

        static async Task ViewOrderHistoryAsync(User customer, IOrderRepository orderRepo)
        {
            var orders = await orderRepo.GetOrdersByUserIdAsync(customer.Id);

            if (orders == null || !orders.Any())
            {
                Console.WriteLine("You have no orders yet.");
                return;
            }

            Console.WriteLine("\n=== Your Order History ===");
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.Id}, Date: {order.CreatedAt}");
                decimal orderTotal = 0;
                foreach (var item in order.Items)
                {
                    decimal itemTotal = item.Product.Price * item.Quantity;
                    orderTotal += itemTotal;
                    Console.WriteLine($" - {item.Product.Name} x {item.Quantity} = {itemTotal:C}");
                }
                Console.WriteLine($" Total: {orderTotal:C}\n");
            }
        }
    }
}
