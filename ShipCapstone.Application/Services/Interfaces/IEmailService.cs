using ShipCapstone.Domain.Models.Emails;

namespace ShipCapstone.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}