using System.ComponentModel.DataAnnotations;

namespace InventoryOrderSystem.Data.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "Customer"; // "Customer" or "Admin"

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
