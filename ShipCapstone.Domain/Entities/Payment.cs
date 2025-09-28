using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Payment : EntityAuditBase<Guid>
{
    public EPaymentMethod PaymentMethod { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
}