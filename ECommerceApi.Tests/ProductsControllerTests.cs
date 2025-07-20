using Xunit;
using Moq;
using ECommerceApi.Repositories;
using ECommerceApi.Controllers;
using ECommerceApi.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class ProductsControllerTests
{

    [Fact]
    public async Task GetProducts_ReturnsProducts()
    {
        var mockProducts = new List<Product>
        {
            new Product { Name = "Apple iPhone 14", Description = "Latest Apple smartphone", Price = 999.99m, Quantity = 50 },
            new Product { Name = "Samsung Galaxy S23", Description = "Flagship Samsung phone", Price = 899.99m, Quantity = 40 },
            new Product { Name = "Sony WH-1000XM5", Description = "Noise cancelling headphones", Price = 349.99m, Quantity = 30 }
        };

        var mockProductRepo = new Mock<IProductRepository>();
        mockProductRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(mockProducts);

        var mockRedisRepo = new Mock<IRedisRepository>();
        mockRedisRepo.Setup(r => r.ProductsExistAsync()).ReturnsAsync(false);

        var productsController = new ProductsController(mockProductRepo.Object, mockRedisRepo.Object);

        var actionResult = await productsController.GetProductsAsync();

        mockProductRepo.Verify(r => r.GetAllAsync(), Times.Once());

        mockRedisRepo.Verify(r => r.ProductsExistAsync(), Times.Once());
        mockRedisRepo.Verify(r => r.SetProductsAsync(mockProducts, TimeSpan.FromMinutes(5)), Times.Once());
        mockRedisRepo.Verify(r => r.GetProductsAsync(), Times.Never());

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(mockProducts.Count, returnedProducts.Count());
    }

    [Fact]
    public async Task GetProducts_Twice_ReturnsProductsFromCache()
    {
        var mockProducts = new List<Product>
        {
            new Product { Name = "Apple iPhone 14", Description = "Latest Apple smartphone", Price = 999.99m, Quantity = 50 },
            new Product { Name = "Samsung Galaxy S23", Description = "Flagship Samsung phone", Price = 899.99m, Quantity = 40 },
            new Product { Name = "Sony WH-1000XM5", Description = "Noise cancelling headphones", Price = 349.99m, Quantity = 30 }
        };
        var mockProductRepo = new Mock<IProductRepository>();
        mockProductRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(mockProducts);
        var mockRedisRepo = new Mock<IRedisRepository>();
        //simulate two calls and products get cached
        int callCount = 0;
        mockRedisRepo.Setup(r => r.ProductsExistAsync()).ReturnsAsync(() =>
        {
            callCount++;
            return callCount > 1;
        });
        mockRedisRepo.Setup(r => r.GetProductsAsync()).ReturnsAsync(mockProducts);
        var controller = new ProductsController(mockProductRepo.Object, mockRedisRepo.Object);
        
        var firstResult = await controller.GetProductsAsync();
        var secondResult = await controller.GetProductsAsync();

        mockProductRepo.Verify(r => r.GetAllAsync(), Times.Once);
        mockRedisRepo.Verify(r => r.ProductsExistAsync(), Times.Exactly(2));
        mockRedisRepo.Verify(r => r.SetProductsAsync(mockProducts, TimeSpan.FromMinutes(5)), Times.Once);
        mockRedisRepo.Verify(r => r.GetProductsAsync(), Times.Once);
        var okResult1 = Assert.IsType<OkObjectResult>(firstResult.Result);
        var returnedProducts1 = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult1.Value);
        Assert.Equal(mockProducts.Count, returnedProducts1.Count());
        var okResult2 = Assert.IsType<OkObjectResult>(secondResult.Result);
        var returnedProducts2 = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult2.Value);
        Assert.Equal(mockProducts.Count, returnedProducts2.Count());
    }
}