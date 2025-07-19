using ECommerceApi.Models;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IRedisRepository _redisRepository;

    public ProductsController(IProductRepository productRepository, IRedisRepository redisRepository)
    {
        _productRepository = productRepository;
        _redisRepository = redisRepository;
    }

    [HttpGet]
    //[AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        if (await _redisRepository.ProductsExistAsync())
        {
            Console.WriteLine("getting products from cache (redis)");
            var cachedProducts = await _redisRepository.GetProductsAsync();
            return Ok(cachedProducts);
        }

        Console.WriteLine("getting products from primary db (sql)");
        var products = await _productRepository.GetAllAsync();
        Console.WriteLine("adding products to cache (redis)");
        _redisRepository.SetProductsAsync(products.ToList<Product>(), TimeSpan.FromMinutes(5));
        return Ok(products);
    }

    [HttpGet("{id}")]
    //[AllowAnonymous]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await _productRepository.AddAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id) return BadRequest();

        var existing = await _productRepository.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Quantity = product.Quantity;

        await _productRepository.UpdateAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();

        await _productRepository.DeleteAsync(product);

        return NoContent();
    }
}