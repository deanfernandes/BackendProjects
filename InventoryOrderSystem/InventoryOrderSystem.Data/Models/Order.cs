using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryOrderSystem.Data.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }

    public User? User { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
