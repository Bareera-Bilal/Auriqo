using System;

namespace Auriqo_Web_Api_Backend.Interfaces;

public interface IMailService
{
public Task SendEmailAsync(string emailAddress , string subject ,  string body, bool isHtml = true );
}