using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Ship : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? RegisterNo { get; set; }
    public int? BuildYear { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CaptainId { get; set; }
    public virtual Account Account { get; set; }
    public virtual Account? Captain { get; set; }
    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
    public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<ShipPortHistory>? ShipPortHistories { get; set; } = new List<ShipPortHistory>();
    public virtual ICollection<ReportProblem>? ReportProblems { get; set; } = new List<ReportProblem>();
}