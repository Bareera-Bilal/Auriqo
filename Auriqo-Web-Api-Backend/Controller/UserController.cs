using Microsoft.AspNetCore.Mvc;
using Auriqo_Web_Api_Backend.Interfaces;
using Auriqo_Web_Api_Backend.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;

namespace Auriqo_Web_Api_Backend.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {

        private readonly SqlDbContext dbContext;
        private readonly ITokenService tokenService;

        public UserController(SqlDbContext dbContext, ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }


        //----------------------------- REGISTER LOGIC ----------------------------- 

        [HttpPost("Register")]
        public async Task<IActionResult> Register(User req)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(new
                {
                    message = "All the feilds are required"
                });
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

            if (user != null)
            {
                return BadRequest(new
                {
                    message = "User Already Registered,Kindly Login "
                });
            }

            var passEncrypt = BCrypt.Net.BCrypt.HashPassword(req.Password);
            req.Password = passEncrypt;

            await dbContext.Users.AddAsync(req);
            await dbContext.SaveChangesAsync();

            //It creates a token for a user with their ID, email, username (or “GuestUser” if none is provided), and the number 23 as extra info, then saves that token in the variable
            var token = tokenService.CreateToken(req.UserId, req.Email, req.Username ?? "GuestUser", 23);

            // We want to send the token inside cookies.But for now, we are sending the message and token inside a JSON object.

            return Ok(new
            {
                message = "Register Successful",
                payload = req,
                authToken = token
            });
        }

        //----------------------------- REGISTER LOGIC COMPLETED -----------------------------       



        //----------------------------- LOGIN LOGIC ----------------------------- 

        [HttpPost("Login")]

        public async Task<IActionResult> Login(User req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new
                    {
                        message = " ALL CREDENTIALS REQUIRED! "
                    });
                }

                var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

                if (existingUser == null)
                {
                    return StatusCode(404, new { message = "USER IS NOT REGISTERED, PLEASE REGISTER" });
                }


                var checkpass = BCrypt.Net.BCrypt.Verify(req.Password, existingUser.Password);

                if (checkpass)
                {
                    var token = tokenService.CreateToken(existingUser.UserId, req.Email, existingUser.Username ?? "Auriqo", 60 * 24);


                    HttpContext.Response.Cookies.Append("Auriqo-Authorization-Token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddHours(24)
                    });

                    return Ok(new
                    {
                        message = "YOU ARE SUCESSFULLY LOGGED IN",
                        payload = token
                    });


                }
                else
                {
                    return StatusCode(400, new { message = "PASSWORD INCORRECT" });
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Internal server Error!" });
            }

        }

        // ----------------------------- LOGIN LOGIC COMPLETED -----------------------------     


        //----------------------------- VERIFY LOGIC -----------------------------     
        [HttpGet("Verify")]

        public IActionResult Verify(string token)
        {

            if (token == null)
            {

                return StatusCode(401, new { message = "UNAUTHORIZED TO ACCESS" });

            }

            var VerifiedUserId = tokenService.VerifyTokenAndGetId(token);

            if (Guid.Empty != VerifiedUserId)
            {
                return StatusCode (200, new{meassage="VERIFIED SUCCESSFULLY", payload = VerifiedUserId});
            }

            else{
                return StatusCode(403, new{message="NO PERMISSION GRANTED TO ACCESS"});
            }

        }

        //----------------------------- VERIFY LOGIC COMPLETED-----------------------------   



        

        //----------------------------- FORGOTPASS LOGIC ----------------------------- 
        [HttpGet("ForgotPass")]  

        public IActionResult ForgotPass(string email){
            return Ok (new{
                message ="EMAIL SENT SUCCESSFULLY, PLEASE CHECK YOU EMAIL"
            });
        }

 //----------------------------- FORGOTPASS LOGIC COMPLETED----------------------------- 
    }
}