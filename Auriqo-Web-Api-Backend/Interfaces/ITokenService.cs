using System;
using Auriqo_Web_Api_Backend.Enum;

namespace Auriqo_Web_Api_Backend.Interfaces;

public interface ITokenService
{
public string CreateToken(Guid userId, string email, string username,  int time);

public string CreateToken(Guid userId, string email, string username, UserType type, int time);

public Guid VerifyTokenAndGetId(string token);



}
