using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_API_Version_6.Controllers;
using Product_API_Version_6.Database_Setting;
using Product_API_Version_6.Models;

namespace Product_Test_Project
{
    public class Product_Test_Project
    {
        public class ProductsControllerTests
        {
            private DbContextOptions<ShopContext> _options;

            public ProductsControllerTests()
            {
                _options = new DbContextOptionsBuilder<ShopContext>()
                    .UseInMemoryDatabase(databaseName: "Shop")
                    .Options;
            }

            // 1.Test that the action returns a 200 (OK) status code when the product exists.
            [Fact]
            public async Task GetAllProducts_ReturnsOkResult()
            {
                // Arrange
                using (var context = new ShopContext(_options))
                {
                    context.Products.AddRange(new List<Product>
            {
                new Product { Id = 1, Name = "Product 1",Sku = "hsnd", CategoryId= 1, Price = 50, IsAvailable=true, Description= " Hello world"},
                new Product { Id = 2, Name = "Product2",Sku = "hsnd", CategoryId= 1, Price = 50, IsAvailable=true, Description= " Hello world"}
                 });
                    context.SaveChanges();
                    var controller = new ProductsController(context);

                    // Act
                    var result = await controller.GetAllProducts();

                    // Assert
                    var okResult = Assert.IsType<OkObjectResult>(result);
                    var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
                    Assert.Equal(3, products.Count());
                }
            }

            //   2.Test that the action returns the correct product when the product exists.
            [Fact]
            public async Task GetProduct_WithValidId_ReturnsOkResult()
            {
                // Arrange
                using (var context = new ShopContext(_options))
                {
                    context.Products.Add(new Product { Id = 3, Name = "Product 3", Sku = "hsnd", CategoryId = 1, Price = 50, IsAvailable = true, Description = " Hello" });

                    context.SaveChanges();
                    var controller = new ProductsController(context);

                    // Act
                    var result = await controller.GetProduct(3);

                    // Assert
                    var okResult = Assert.IsType<OkObjectResult>(result);
                    var product = Assert.IsType<Product>(okResult.Value);
                    Assert.Equal(3, product.Id);
                }
            }
        }
    }
}

/*

    //    3.Test that the action returns a 404 (Not Found) status code when the product does not exist.
            [Fact]
            public async Task GetProduct_WithInvalidId_ReturnsNotFoundResult()
            {
                // Arrange
                using (var context = new ShopContext(_options))
                {
                    var controller = new ProductsController(context);

                    // Act
                    var result = await controller.GetProduct(999);

                    // Assert
                    Assert.IsType<NotFoundResult>(result);
                }
            }
    */