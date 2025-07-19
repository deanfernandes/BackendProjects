using StackExchange.Redis;
using ECommerceApi.Models;
using System.Text.Json;

namespace ECommerceApi.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private const string ProductsKey = "products:all";
        private readonly IDatabase _db;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task<List<Product>?> GetProductsAsync()
        {
            var value = await _db.StringGetAsync(ProductsKey);
            return value.HasValue
                ? JsonSerializer.Deserialize<List<Product>>(value!, _jsonOptions)
                : null;
        }

        public async Task SetProductsAsync(List<Product> products, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(products, _jsonOptions);
            await _db.StringSetAsync(ProductsKey, json, expiry);
        }

        public async Task<bool> ProductsExistAsync()
        {
            return await _db.KeyExistsAsync(ProductsKey);
        }
    }
}
