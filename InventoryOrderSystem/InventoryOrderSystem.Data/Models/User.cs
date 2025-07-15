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

    public string? Email { get; set; }

    [Required]
    public string Salt { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
