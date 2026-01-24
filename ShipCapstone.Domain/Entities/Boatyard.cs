using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Boatyard : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public string? BankName { get; set; }
    public string? BankNo { get; set; }
    public decimal CommissionFeePercent { get; set; }
    public Guid AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
    public virtual ICollection<DockSlot> DockSlots { get; set; } = new List<DockSlot>();
    public virtual ICollection<BoatyardService>? BoatyardServices { get; set; } = new List<BoatyardService>();
    public virtual ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}