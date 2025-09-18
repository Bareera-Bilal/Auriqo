using System.Net;
using System.Net.Mail;
using Auriqo_Web_Api_Backend.Interfaces;
using Auriqo_Web_Api_Backend.Models;
using Auriqo_Web_Api_Backend.Models.JunctionModels;


using Microsoft.Extensions.Options;

namespace Auriqo_Web_Api_Backend.Services;
public class EmailService : IMailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));

        if (string.IsNullOrEmpty(_settings.SmtpHost))
            throw new ArgumentNullException(nameof(_settings.SmtpHost), "SMTP host is required");
            
        if (string.IsNullOrEmpty(_settings.SmtpUsername))
            throw new ArgumentNullException(nameof(_settings.SmtpUsername), "SMTP username is required");
            
        if (string.IsNullOrEmpty(_settings.SmtpPassword))
            throw new ArgumentNullException(nameof(_settings.SmtpPassword), "SMTP password is required");
    }

    public async Task SendEmailAsync(string emailAddress, string subject, string body, bool isHtml = false)
    {
        if (string.IsNullOrEmpty(emailAddress))
            throw new ArgumentNullException(nameof(emailAddress));

        try
        {
            using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword),
                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SmtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml,
            };
            
            mailMessage.To.Add(emailAddress);

            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new MailServiceException($"Failed to send email to {emailAddress}", ex);
        }
    }
}

// Custom exception for better error handling
public class MailServiceException : Exception
{
    public MailServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}