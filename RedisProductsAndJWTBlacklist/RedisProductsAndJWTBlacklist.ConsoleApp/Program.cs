using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static RedisProductsAndJWTBlacklist.ConsoleApp.Program;

namespace RedisProductsAndJWTBlacklist.ConsoleApp
{
    class Program
    {
        private static IDatabase _db;
        private const string SecretKey = "ThisIsASecureKeyWithAtLeast32Chars!!";

        static void Main()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            _db = redis.GetDatabase();

            string token = Login("bob");
            GetProducts(token);
            Console.WriteLine("Waiting...");
            Thread.Sleep(2000);
            GetProducts(token);
            Logout(token);
            GetProducts(token);
        }

        private static void GetProducts(string token)
        {
            if (IsTokenExpired(token))
            {
                Console.WriteLine("Token expired. Cannot access products.");
                return;
            }

            if (IsTokenBlacklisted(token))
            {
                Console.WriteLine("Token is blacklisted. Access denied.");
                return;
            }

            const string cacheKey = "products:all";

            List<Product> products;
            if (_db.KeyExists(cacheKey))
            {
                Console.WriteLine("Retrieved products from Redis cache.");

                products = JsonSerializer.Deserialize<List<Product>>(_db.StringGet(cacheKey));
                Console.WriteLine("Products:");
                foreach (var p in products)
                {
                    Console.WriteLine($"- {p.Name} (${p.Price})");
                }
                return;
            }

            // Simulate long-running DB fetch
            Console.WriteLine("Fetching products from SQL database...");
            Thread.Sleep(2000); // simulate delay

            products = new List<Product>
            {
                new Product { Id = "1", Name = "Product1", Price = 10.99m },
                new Product { Id = "2", Name = "Product2", Price = 14.50m },
                new Product { Id = "3", Name = "Product3", Price = 9.75m }
            };
            string productsJson = JsonSerializer.Serialize<List<Product>>(products);
            _db.StringSet(cacheKey, productsJson, TimeSpan.FromMinutes(5));

            Console.WriteLine("Products fetched from SQL and cached.");
            Console.WriteLine("Products:");
            foreach (var p in products)
            {
                Console.WriteLine($"- {p.Name} (${p.Price})");

                /*
                string key = $"product:{p.Id}";
                var hashEntries = new HashEntry[]
                {
                    new HashEntry("Id", p.Id),
                    new HashEntry("Name", p.Name),
                    new HashEntry("Price", p.Price.ToString(CultureInfo.InvariantCulture))
                };

                _db.HashSet(key, hashEntries);
                _db.KeyExpire("product:1", TimeSpan.FromMinutes(5));
                */
            }

        }

        private static string Login(string username)
        {
            string issuer = "yourApp";
            string audience = "yourAppUsers";
            string jti = Guid.NewGuid().ToString();
            TimeSpan validity = TimeSpan.FromMinutes(10);

            return GenerateJwtToken(SecretKey, issuer, audience, jti, username, validity);
        }

        private static void Logout(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(expClaim) || !long.TryParse(expClaim, out var expUnix))
            {
                Console.WriteLine("Cannot blacklist token: missing or invalid claims.");
                return;
            }

            var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnix);
            var ttl = expDateTime - DateTimeOffset.UtcNow;

            if (ttl <= TimeSpan.Zero)
            {
                Console.WriteLine("Token already expired; no need to blacklist.");
                return;
            }

            _db.StringSet($"blacklist:{jti}", true, ttl);
            Console.WriteLine($"Blacklisted token jti: {jti} for {ttl.TotalSeconds:N0} seconds");
        }

        private static bool IsTokenBlacklisted(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;

            return !string.IsNullOrEmpty(jti) && _db.KeyExists($"blacklist:{jti}");
        }

        private static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            if (string.IsNullOrEmpty(expClaim) || !long.TryParse(expClaim, out var expUnix))
                return true;

            var exp = DateTimeOffset.FromUnixTimeSeconds(expUnix);
            return DateTimeOffset.UtcNow >= exp;
        }

        public static string GenerateJwtToken(string secretKey, string issuer, string audience, string jti, string userName, TimeSpan tokenValidity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim("name", userName)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(tokenValidity),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

    }
}
