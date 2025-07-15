using InventoryOrderSystem.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InventoryOrderSystem.Data.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
    Task AddAsync(Order order);
    Task AddOrderWithItemsAsync(Order order, List<OrderItem> items);
}
