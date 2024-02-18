using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_API_Version_6.Database_Setting;
using Product_API_Version_6.Models;
using Product_API_Version_6.Models.Filteration;
using Product_API_Version_6.Models.Pagination;
using Product_API_Version_6.Models.Sorting;
using System.Linq;

namespace Product_API_Version_6.Controllers
{
    //adding API version
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/products")]
   // [Route("api/[controller]")]
    [ApiController]
    public class ProductsV1Controller : ControllerBase
    {
        private readonly ShopContext _context;

        //Constructor
        public ProductsV1Controller(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //Get all the Products

        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            #region GET ALL THE PRODUCTS

            IQueryable<Product> products = _context.Products;

            #region FILTERATION OF THE PRODUCT DATA

            /* using filteration to return the data using minimum and maxmium value  -------------  https://localhost:7268/api/Products?MinPrice=10&MaxPrice=30 */

            if (queryParameters.MinPrice != null)
            {
                products = products.Where(
                    p => p.Price >= queryParameters.MinPrice.Value);
            }
            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(
                    p => p.Price <= queryParameters.MaxPrice.Value);
            }
            #endregion

            #region  SEARCHING THE PRODUCTS USING NAME AND SKU
            //Advance search mechanism challange => specific search -------------- https://localhost:7268/api/Products?SearchTerm=Jac
            if (!string.IsNullOrEmpty(queryParameters.SearchTerm) && !string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                products = products.Where(
                    p => p.Sku.ToLower().Contains(queryParameters.SearchTerm.ToLower()) 
                    ||
                    p.Name.ToLower().Contains(queryParameters.SearchTerm.ToLower())
                    );
                  

            } 


            // Searching the Product by their name and sku -------Looking for substrings----------------- https://localhost:7268/api/Products?Sku=AWMPS

            if (!string.IsNullOrEmpty(queryParameters.Sku) && !string.IsNullOrWhiteSpace(queryParameters.Sku))
            {
                products = products.Where(
                    p => p.Sku == queryParameters.Sku);
            }

            if(!string.IsNullOrEmpty(queryParameters.Name) && !string.IsNullOrWhiteSpace(queryParameters.Name))
            {
                products = products.Where(
                    p=> p.Name.ToLower().Contains(
                        queryParameters.Name.ToLower())
                    );    
            }

            #endregion

            #region   SORTING THE PRODUCT DATA

            //Sorting is a method that helps to ascending and descending the order by arbiaraty parameters -------------------- https://localhost:7268/api/Products?SortBy=Price&SortOrder=desc

            if (!string.IsNullOrEmpty(queryParameters.SortBy) &&  !string.IsNullOrWhiteSpace(queryParameters.SortBy))
            {
                //check the product class does has a properties is not null or not

                if(typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    products = products.OrderByCustom(
                        queryParameters.SortBy,
                        queryParameters.SortOrder);
                }
            }

            #endregion

            #region PAGINATION FOR THE PRODUCT DATA
            /* using Pagination to return the data => url should be like ------------  https://localhost:7268/api/Products?size=15&page=2          */
            products = products
                .Skip(queryParameters.Size *(queryParameters.Page -1))
                .Take(queryParameters.Size);
            return Ok(products.ToArrayAsync());

            //General returning Method
            //return Ok(await _context.Products.ToArrayAsync());
            #endregion

            #endregion
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

    //controller version : 2
    //adding API version
    [ApiVersion("2.0")]
    [Route("v{v:apiVersion}/products")]
    // [Route("api/[controller]")]
    [ApiController]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly ShopContext _context;

        //Constructor
        public ProductsV2Controller(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //Get all the Products

        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            #region GET ALL THE PRODUCTS
            //Returns the available Products 
            IQueryable<Product> products = _context.Products.Where(p => p.IsAvailable == true);

            // IQueryable<Product> products = _context.Products;

            #region FILTERATION OF THE PRODUCT DATA

            /* using filteration to return the data using minimum and maxmium value  -------------  https://localhost:7268/api/Products?MinPrice=10&MaxPrice=30 */

            if (queryParameters.MinPrice != null)
            {
                products = products.Where(
                    p => p.Price >= queryParameters.MinPrice.Value);
            }
            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(
                    p => p.Price <= queryParameters.MaxPrice.Value);
            }
            #endregion

            #region  SEARCHING THE PRODUCTS USING NAME AND SKU
            //Advance search mechanism challange => specific search -------------- https://localhost:7268/api/Products?SearchTerm=Jac
            if (!string.IsNullOrEmpty(queryParameters.SearchTerm) && !string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                products = products.Where(
                    p => p.Sku.ToLower().Contains(queryParameters.SearchTerm.ToLower())
                    ||
                    p.Name.ToLower().Contains(queryParameters.SearchTerm.ToLower())
                    );


            }


            // Searching the Product by their name and sku -------Looking for substrings----------------- https://localhost:7268/api/Products?Sku=AWMPS

            if (!string.IsNullOrEmpty(queryParameters.Sku) && !string.IsNullOrWhiteSpace(queryParameters.Sku))
            {
                products = products.Where(
                    p => p.Sku == queryParameters.Sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name) && !string.IsNullOrWhiteSpace(queryParameters.Name))
            {
                products = products.Where(
                    p => p.Name.ToLower().Contains(
                        queryParameters.Name.ToLower())
                    );
            }

            #endregion

            #region   SORTING THE PRODUCT DATA

            //Sorting is a method that helps to ascending and descending the order by arbiaraty parameters -------------------- https://localhost:7268/api/Products?SortBy=Price&SortOrder=desc

            if (!string.IsNullOrEmpty(queryParameters.SortBy) && !string.IsNullOrWhiteSpace(queryParameters.SortBy))
            {
                //check the product class does has a properties is not null or not

                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    products = products.OrderByCustom(
                        queryParameters.SortBy,
                        queryParameters.SortOrder);
                }
            }

            #endregion

            #region PAGINATION FOR THE PRODUCT DATA
            /* using Pagination to return the data => url should be like ------------  https://localhost:7268/api/Products?size=15&page=2          */
            products = products
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);
            return Ok(products.ToArrayAsync());

            //General returning Method
            //return Ok(await _context.Products.ToArrayAsync());
            #endregion

            #endregion
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