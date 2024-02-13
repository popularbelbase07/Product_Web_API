using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_API_Version_6.Database_Setting;
using Product_API_Version_6.Models;

namespace Product_API_Version_6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;

        //Constructor
        public ProductsController(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //Get all the Products
        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            return Ok(await _context.Products.ToArrayAsync());
        }

        //Get specific Product using ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //Post the data to the database
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var post = _context.Products.Add(product);
            await post.Context.SaveChangesAsync();

            return CreatedAtAction(

                "GetProduct",
                new { id = product.Id },
                product);
        }

        //Put
        [HttpPut]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        [HttpPost]
        [Route("DeleteAll")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var products = new List<Product>();

            foreach (int id in ids)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }
                products.Add(product);
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();

            return Ok(products);
        }

        // PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product productUpdate)
        {
            // Implement logic to update the product with the given ID
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Update product properties based on productUpdate
            if (productUpdate.Name != null)
            {
                product.Name = productUpdate.Name;
            }
            if (productUpdate.Sku != null)
            {
                product.Sku = productUpdate.Sku;
            }
            if (productUpdate.Description != null)
            {
                product.Description = productUpdate.Description;
            }
            if (productUpdate?.Price != null)
            {
                product.Price = productUpdate.Price;
            }
            if (productUpdate?.IsAvailable != null)
            {
                product.IsAvailable = productUpdate.IsAvailable;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the status code and the Editted Product
            return Ok(productUpdate);
        }
    }
}