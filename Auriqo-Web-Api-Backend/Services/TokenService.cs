using System;
using System.IdentityModel.Tokens.Jwt;
using Auriqo_Web_Api_Backend.Interfaces;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Auriqo_Web_Api_Backend.Enum;


namespace Auriqo_Web_Api_Backend.Services;

public class TokenService : ITokenService// inheritance 

{
    private readonly string _secretKey;     // private feild 


    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("SecretKey is not configured.");
    }

  // code snippets 
    public string CreateToken(Guid userId, string email, string username, int time)
    {
        var tokenHandler = new JwtSecurityTokenHandler();   // intializing new instance of  JwtSecurityTokenHandler

        var key = Encoding.ASCII.GetBytes(_secretKey);  /// secret in ascii format 

        var payload = new SecurityTokenDescriptor      // creation of payload
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, username)
            ]),

            Expires = DateTime.UtcNow.AddMinutes(time),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(payload);     // creation of token 

        return tokenHandler.WriteToken(token);        // returning of token 
    }

    public string CreateToken(Guid userId, string email, string username, UserType type, int time)
    {
        throw new NotImplementedException();
    }

    public Guid VerifyTokenAndGetId(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_secretKey);


            var validationParameters = new TokenValidationParameters   
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var validatToken = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);


            var userId = validatToken.FindFirst(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                return Guid.Parse(userId.Value);
            }
            else
            {
                throw new Exception("User ID not found in token.");
            }
        }
        catch (SecurityTokenExpiredException)
        {
            throw new Exception("Token has expired.");
        }
        catch (Exception ex)
        {
            throw new Exception("Token validation failed: " + ex.Message);
        }
       
    }


}