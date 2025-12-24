using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Transaction : EntityAuditBase<Guid>
{
    public decimal Amount { get; set; }
    public string TransactionCode { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? BoatyardId { get; set; }
    public EPaymentType Type { get; set; }
    public ETransactionStatus Status { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? BookingId { get; set; }
    public virtual Order? Order { get; set; }
    public virtual Booking? Booking { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual Boatyard? Boatyard { get; set; }
}