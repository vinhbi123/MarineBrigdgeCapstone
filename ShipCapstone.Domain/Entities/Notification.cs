using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Notification : EntityAuditBase<Guid>
{
    public string Title { get; set; }
    public bool IsRead { get; set; }
    public string? Content { get; set; }
    public Guid AccountId { get; set; }
    
    public virtual Account Account { get; set; }
}