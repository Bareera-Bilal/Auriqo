using Auriqo_Web_Api_Backend.Interfaces;
using Auriqo_Web_Api_Backend.Models.DomainModels;
using Auriqo_Web_Api_Backend.Models.JunctionModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Auriqo_Web_Api_Backend.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class CartController : ControllerBase
    {

        private readonly SqlDbContext dbContext;
        private readonly ITokenService tokenService;

        public CartController(SqlDbContext dbContext, ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }



        [HttpPost("addtocart")]
        public async Task<IActionResult> AddToCart(Guid productId, int qty)
        {

            try
            {

                //We have to fetch token first
                var token = HttpContext.Request.Cookies["Auriqo-Authorization-Token"];

                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(403, new { message = " SESSION EXPIRED, KINDLY LOGIN AGAIN" });
                }

                //We have to verify token
                var userId = tokenService.VerifyTokenAndGetId(token);

                if (userId == Guid.Empty)
                {
                    return Unauthorized(new { message = "UMAUTHORIZED TO ACCESS, KINDLY LOGIN AGAIN" })

                }

                //FIND PRODUCT BY IT'S ID
                var product = await dbContext.Products.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(new { message = "PRODUCT NOT FOUND" });
                }

                var cart = await dbContext.Carts.Include(cart => cart.CartProducts).FirstOrDefaultAsync(cart => cart.UserId == userId);

                if (cart == null)
                {

                    var newCart = new Cart
                    {
                        UserId = userId,
                        CartTotal = product.ProductPrice * qty

                    };

                    var CartProduct = new CartProduct
                    {
                        CartId = newCart.CartId,
                        ProductId = productId,
                        Quantity = qty,
                        ProductPrice = product.ProductPrice
                    };

                    await dbContext.Carts.AddAsync(newCart);
                    await dbContext.CartProducts.AddAsync(cartProduct);
                    await dbContext.SaveChangesAsync();
                    return Ok(new { message = "cart created", payload = newCart });

                }

                else
                {
                    var existingCartProduct = await dbContext.CartProducts.FirstOrDefaultAsync(cp => cp.ProductId == productId && cp.CartId == cart.CartId);

                    if (existingCartProduct != null)
                    {
                        existingCartProduct.Quantity += qty;
                    }

                    else
                    {
                        var cartProduct = new CartProduct
                        {
                            CartId = cart.CartId,
                            ProductId = productId,
                            Quantity = qty,
                            ProductPrice = product.ProductPrice
                        };

                        await dbContext.CartProducts.AddAsync(cartProduct);

                    }

                    cart.CartTotal += product.ProductPrice * qty;
                    await dbContext.SaveChangesAsync();

                    // UPDATE PENDING
                    return Ok(new { message = "CART UPDATED SUCCESSFULLY", payload = cart });

                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPost("RemoveFromCart")]

        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {

            try
            {
                var token = HttpContext.Request.Cookies["Auriqo-Authorization-Token"];

                if (token == null)
                {
                    return StatusCode(403, new { message = "PLEASE LOGIN" });
                }

                var userId = tokenService.VerifyTokenAndGetId(token);

                if (userId == Guid.Empty)
                {
                    return StatusCode(404, new { message = "PLEASE LOGIN " });
                }


                //CART FETCHING    SUB QUERY CART PRODUCT FETCH
                var cart = await dbContext.Carts.Include(cart => cart.CartProducts).FirstOrDefaultAsync(cart => cart.UserId == userId);

                if (cart == null)
                {
                    return NotFound(new { message = "CART NOT FOUND" });
                }

                //BUFFER //LIST //QUERY IS VERY FAST
                var cartproduct = cart.CartProducts.FirstOrDefault(cp => cp.CartId == cart.CartId && cp.ProductId == productId);

                if (cartproduct == null)
                {
                    return NotFound(new { message = "CART ITEM NOT FOUND" });
                }

                var remove = dbContext.CartProducts.Remove(cartproduct);

                if (remove != null)
                {
                    cart.CartTotal -= cartproduct.ProductPrice * cartproduct.Quantity;
                    await dbContext.SaveChangesAsync();
                    return Ok(new { message = "ITEM REMOVED FROM CART", payload = cart });
                }
                else
                {
                    return BadRequest(new { message = "SOMETHING WENT WRONG" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }





    }

}