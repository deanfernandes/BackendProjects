using ECommerceApi.Models;

public interface IRedisRepository
{
    Task<List<Product>?> GetProductsAsync();
    Task SetProductsAsync(List<Product> products, TimeSpan? expiry = null);
    Task<bool> ProductsExistAsync();
}