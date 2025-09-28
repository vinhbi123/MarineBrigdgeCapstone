using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Review : EntityAuditBase<Guid>
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public Guid AccountId { get; set; }
    public Guid ProductId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual Product Product { get; set; }
}