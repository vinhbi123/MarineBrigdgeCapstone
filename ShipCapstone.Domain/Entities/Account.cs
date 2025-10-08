using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Account : EntityAuditBase<Guid>
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public ERole Role { get; set; }

    public virtual Ship? Ship { get; set; }
    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
    public virtual Boatyard? Boatyard { get; set; }
    public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Complaint>? Complaints { get; set; } = new List<Complaint>();
    public virtual ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Payment>? Payments { get; set; } = new List<Payment>();
}