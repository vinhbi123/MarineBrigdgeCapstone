using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Order : EntityAuditBase<Guid>
{
    public decimal TotalAmount { get; set; }
    public Guid ShipId { get; set; }
    
    public virtual Ship Ship { get; set; }

    public virtual ICollection<Complaint>? Complaints { get; set; } = new List<Complaint>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}