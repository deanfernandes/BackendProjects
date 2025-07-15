﻿using System.ComponentModel.DataAnnotations;

namespace InventoryOrderSystem.Data.Models;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
