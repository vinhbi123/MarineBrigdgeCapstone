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
    public string? ShipName { get; set; }
    public string? ShipOwnerName { get; set; }
    public string? ShipOwnerPhoneNumber { get; set; }
    public Guid DockSlotId { get; set; }
    public string? DockSlotName { get; set; }
    public List<BookingServiceDetailResponse>? Services { get; set; }
}

public class BookingServiceDetailResponse
{
    public Guid Id { get; set; }
    public string? TypeService { get; set; }
    public decimal Price { get; set; }
}