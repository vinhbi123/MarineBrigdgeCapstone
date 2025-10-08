using Microsoft.Extensions.Options;
using MimeKit;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Models.Emails;
using ShipCapstone.Domain.Models.Settings;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ShipCapstone.Application.Services.Implements;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    
    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
    }
    
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var builder = new BodyBuilder() { HtmlBody = emailMessage.Body };

        var email = new MimeMessage
        {
            Subject = emailMessage.Subject,
            Body = builder.ToMessageBody()
        };

        email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        email.To.Add(new MailboxAddress(emailMessage.ToAddress.Split("@")[0], emailMessage.ToAddress));
        
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(email);
        }
        catch
        {
            await client.DisconnectAsync(true);
            client.Dispose();
            throw;
        }
    }
}