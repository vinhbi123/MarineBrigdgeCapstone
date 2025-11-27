using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Models.Booking;

public class GetBookingByIdResponse
{
    public Guid Id { get; set; }
    public EBookingStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public EBookingType Type { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid ShipId { get; set; }
    public Guid DockSlotId { get; set; }
}