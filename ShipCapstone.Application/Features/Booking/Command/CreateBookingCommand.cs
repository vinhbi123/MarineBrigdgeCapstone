using Mediator;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Booking;
using ShipCapstone.Domain.Models.Common;
using System;
using System.Collections.Generic;

namespace ShipCapstone.Application.Features.Bookings.Command.CreateBooking
{
    public class CreateBookingCommand : IRequest<ApiResponse>
    {
        public Guid ShipId { get; set; }
        public Guid DockSlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public EBookingType Type { get; set; }
        public List<Guid> Services { get; set; } = new();
    }
}
