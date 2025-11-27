using Mediator;
using Net.payOS.Types;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Payments.Command.PaymentWebhook;

public class ConfirmWebhookCommand : IRequest<ApiResponse>
{
    public WebhookType Payload { get; set; }
}