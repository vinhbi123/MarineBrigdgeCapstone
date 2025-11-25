using ShipCapstone.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ShipCapstone.Domain.Models.Booking
{
    public class CreateBookingResponse
    {
        public Guid Id { get; set; }
        public Guid ShipId { get; set; }
        public Guid DockSlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public EBookingType Type { get; set; }
        public decimal TotalAmount { get; set; }
        public EBookingStatus Status { get; set; }
        public List<Guid> Services { get; set; } = new();
    }
}
    