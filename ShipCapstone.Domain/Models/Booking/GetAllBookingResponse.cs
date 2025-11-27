using ShipCapstone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipCapstone.Domain.Models.Booking
{
    public class GetAllBookingResponse
    {
        public Guid Id { get; set; }
        public Guid ShipId { get; set; }
        public string? ShipName { get; set; }
        public string? ShipOwnerName { get; set; }
        public string? ShipOwnerPhoneNumber { get; set; }
        public Guid DockSlotId { get; set; }
        public string? DockSlotName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public EBookingType Type { get; set; }
        public decimal TotalAmount { get; set; }
        public EBookingStatus Status { get; set; }
        public List<Guid> Services { get; set; } = new();
    }
}
