using System.Threading.Tasks;
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


        [HttpGet("archive")]

        public async Task<IActionResult> ArchiveProduct(Guid productId){
            
            try
            {
                var product = await dbContext.Products.FindAsync(productId);
                if(product == null){
                    return NotFound(new{message="PRODUCT NOT FIND", productId});
                }

                if(product.IsArchived == false && product.IsAvailable == true){
                    product.IsArchived = true;
                    product.IsAvailable = false;
                    product.UpdatedAt = DateTime.Now;
                    await dbContext.SaveChangesAsync();

                }

                else{
                    return BadRequest(new{message="PRODUCT IS ALREADY IN ARCHIVE"});
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }











    }

}