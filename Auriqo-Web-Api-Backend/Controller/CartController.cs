using Auriqo_Web_Api_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Auriqo_Web_Api_Backend.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class CartController : ControllerBase
    {

        private readonly SqlDbContext dbContext;
        private readonly ITokenService tokenService;

        public CartController (SqlDbContext dbContext, ITokenService tokenService){
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }




        

    }

}