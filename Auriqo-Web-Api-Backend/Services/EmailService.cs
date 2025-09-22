using System.Net;
using System.Net.Mail;
using Auriqo_Web_Api_Backend.Interfaces;
using Auriqo_Web_Api_Backend.Models;
using Auriqo_Web_Api_Backend.Models.JunctionModels;


using Microsoft.Extensions.Options;

namespace Auriqo_Web_Api_Backend.Services;
public class EmailService : IMailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailService(IConfiguration configuration)
    {
        _smtpHost = configuration["Smtp:Host"] ?? throw new InvalidOperationException("SMTP host is not configured.");

        _smtpPort = int.TryParse(configuration["Smtp:Port"], out var port) ? port : 587;

        _smtpUsername = configuration["Smtp:Username"] ?? throw new InvalidOperationException("SMTP username is not configured.");

        _smtpPassword = configuration["Smtp:Password"]?? throw new InvalidOperationException("SMTP password is not configured.");
    }

    public async Task SendEmailAsync(string emailAddress, string subject, string body, bool isHtml = true)
    {
        using var client = new SmtpClient(_smtpHost, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("bareerabilal03@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };



        mailMessage.To.Add(emailAddress);

        try
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
        }
    }

}