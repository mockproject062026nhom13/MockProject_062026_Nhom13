using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NursingHome.Application.Abstractions.Services;

namespace NursingHome.Infrastructure.Services.Messaging;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        var host = _configuration["EmailSettings:Host"];
        var portStr = _configuration["EmailSettings:Port"];
        var username = _configuration["EmailSettings:Username"];
        var password = _configuration["EmailSettings:Password"];
        var enableSslStr = _configuration["EmailSettings:UseSSL"];
        var fromEmail = _configuration["EmailSettings:SenderEmail"] ?? username;
        var senderName = _configuration["EmailSettings:SenderName"] ?? "Nursing Home";

        // Bỏ qua gửi mail thật nếu chưa config Host (tránh crash khi dev chưa cấu hình .env)
        if (string.IsNullOrEmpty(host))
        {
            _logger.LogWarning("SMTP Configuration is missing. Skipping real email sending. OTP details: Subject='{Subject}', To='{To}'", subject, toEmail);
            return;
        }

        int port = int.TryParse(portStr, out int p) ? p : 587;
        bool enableSsl = bool.TryParse(enableSslStr, out bool ssl) ? ssl : true;

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(senderName, fromEmail));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;
        
        emailMessage.Body = new TextPart(TextFormat.Html) 
        { 
            Text = body 
        };

        using var client = new SmtpClient();

        try
        {
            var secureSocketOptions = enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
            await client.ConnectAsync(host, port, secureSocketOptions, cancellationToken);
            await client.AuthenticateAsync(username, password, cancellationToken);
            await client.SendAsync(emailMessage, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
            throw;
        }
    }
}
