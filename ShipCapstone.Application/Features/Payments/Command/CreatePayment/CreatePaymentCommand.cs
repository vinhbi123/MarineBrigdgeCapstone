using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Payments.Command.CreatePayment;

public class CreatePaymentCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public EPaymentType Type { get; set; }
    public string Address { get; set; }
}