using ECommerceApi.Models;

public interface IRedisRepository
{
    Task<List<Product>?> GetProductsAsync();
    Task SetProductsAsync(List<Product> products, TimeSpan? expiry = null);
    Task<bool> ProductsExistAsync();
    Task BlacklistTokenAsync(string token, TimeSpan expiry);
    Task<bool> IsTokenBlacklistedAsync(string token);
}