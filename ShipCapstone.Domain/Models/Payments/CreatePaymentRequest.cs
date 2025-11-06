using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Models.Payments;

public class CreatePaymentRequest
{
    public Account Account { get; set; }
    public object PaymentObject { get; set; }
    public EPaymentType Type { get; set; }
    public string Address { get; set; }
}