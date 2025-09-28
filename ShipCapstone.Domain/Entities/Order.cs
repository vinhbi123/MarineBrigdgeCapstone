using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Order : EntityAuditBase<Guid>
{
    public decimal TotalAmount { get; set; }
    public Guid AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual ICollection<Delivery>? Deliveries { get; set; } = new List<Delivery>();
    public virtual ICollection<Complaint>? Complaints { get; set; } = new List<Complaint>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}