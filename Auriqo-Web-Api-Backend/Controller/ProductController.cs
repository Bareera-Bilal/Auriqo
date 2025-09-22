using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Auriqo_Web_Api_Backend.Middlewares;
using Auriqo_Web_Api_Backend.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auriqo_Web_Api_Backend.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly SqlDbContext dbContext;

        public ProductController(SqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "ALL THE INFORMATION OF PRODUCT IS REQUIRED" });

                }

                await dbContext.Products.AddAsync(product);
                await dbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = "PRODUCT ADDED SUCCESSFULLY"
                });

            }
            catch (System.Exception)
            {
                return StatusCode(500, new { message = "INTERNAL SERVER ERROR" });

            }
        }

        [Authorize]
        [HttpGet("archive")]

        public async Task<IActionResult> ArchiveProduct(Guid productId)
        {

            try
            {
                var product = await dbContext.Products.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(new { message = "PRODUCT NOT FIND", productId });
                }

                if (product.IsArchived == false && product.IsAvailable == true)
                {
                    product.IsArchived = true;
                    product.IsAvailable = false;
                    product.UpdatedAt = DateTime.Now;
                    await dbContext.SaveChangesAsync();

                }

                else
                {

                    return BadRequest(new { message = "PRODUCT IS ALREADY IN ARCHIVE" });
                }

                return Ok(new
                {
                    message = "PRODUCT ARCHIVED!"
                });
            }
            catch (System.Exception)
            {

                throw;
            }
        }

         [Authorize]
        [HttpGet("unarchive")]

        public async Task<IActionResult> UnArchiveProduct(Guid productId)
        {
            try
            {
                var product = await dbContext.Products.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(new { message = "PRODUCT NOT FOUND" });
                }

                if (product.IsArchived == true && product.IsAvailable == false)
                {
                    product.IsArchived = false;
                    product.IsAvailable = true;
                    product.UpdatedAt = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                }

                else
                {
                    return BadRequest(new { message = "PRODUCT IS ALREADY IN UNARCHIVED LIST" });
                }

                return Ok(new
                {
                    message = "PRODUCT UNARCHIVED",
                    payload = product
                });

            }
            catch (System.Exception)
            {

                throw;
            }

        }


         [Authorize]
        [HttpGet("delete")]

        public async Task<IActionResult> DeleteProduct(Guid productId)
        {

            try
            {
                var product = await dbContext.Products.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(new { message = "PRODUCT NOT FOUND", productId });
                }

                if (product.IsArchived == true && product.IsAvailable == false)
                {
                    product.IsArchived = false;
                    product.UpdatedAt = DateTime.Now;

                    await dbContext.SaveChangesAsync();
                }

                else
                {
                    return BadRequest(new { message = "PRODUCT IS SHIFTED TO RECYCLE BIN" });
                }

                return Ok();
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [Authorize]
        [HttpPut("update")]

        public async Task<IActionResult> UpdateProduct(Guid productId, Product product)
        {
            try
            {
                var existingproduct = await dbContext.Products.FindAsync(productId);

                if (existingproduct == null)
                {
                    return NotFound(new { message = "PRODUCT NOT FOUND" });

                }

                existingproduct.ProductName = product.ProductName;
                existingproduct.ProductDescription = product.ProductDescription;
                existingproduct.ProductImage = product.ProductImage;
                existingproduct.ProductStock = product.ProductStock;
                existingproduct.ProductPrice = product.ProductPrice;
                existingproduct.Size = product.Size;
                existingproduct.Color = product.Color;
                existingproduct.Weight = product.Weight;
                existingproduct.Category = product.Category;
                existingproduct.UpdatedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(new { message = "PRODUCT UPDATED SUCCESSFULLY", payload = existingproduct });



            }
            catch (System.Exception)
            {

                throw;
            }

        }


        [HttpGet("getAll")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await dbContext.Products.Where(p => p.IsAvailable == true).ToListAsync();

                return Ok(new { message = $"{products.Count} PRODUCTS ARE FOUND", payload = products });
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [HttpGet("getbyId")]

        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                var product = await dbContext.Products.FindAsync(productId);
                return Ok(new { message = " 1 PRODUCT FOUND", payload = product });


            }
            catch (System.Exception)
            {

                throw;
            }

        }




    }

}