using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Order : EntityAuditBase<Guid>
{
    public decimal TotalAmount { get; set; }
    public EOrderStatus Status { get; set; }
    public Guid? ShipId { get; set; }
    public Guid? BoatyardId { get; set; }
    
    public virtual Ship? Ship { get; set; }
    public virtual Boatyard? Boatyard { get; set; }

    public virtual ICollection<Complaint>? Complaints { get; set; } = new List<Complaint>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}