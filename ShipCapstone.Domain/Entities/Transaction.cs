using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Transaction : EntityAuditBase<Guid>
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public ETransactionStatus Status { get; set; }
    public Guid OrderId { get; set; }
    
    public virtual Order Order { get; set; }
}