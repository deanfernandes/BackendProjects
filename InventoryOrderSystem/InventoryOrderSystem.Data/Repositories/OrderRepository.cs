using InventoryOrderSystem.Data.Context;
using InventoryOrderSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryOrderSystem.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id) =>
        await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId) =>
        await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync();

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task AddOrderWithItemsAsync(Order order, List<OrderItem> items)
    {
        _context.Orders.Add(order);
        foreach (var item in items)
        {
            order.Items.Add(item);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }
}
