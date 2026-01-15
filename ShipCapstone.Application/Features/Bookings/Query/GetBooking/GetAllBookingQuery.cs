using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Bookings.Query.GetBooking
{
    public class GetAllBookingQuery : IRequest<ApiResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public Guid? BoatyardId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? SortBy { get; set; }
        public bool IsAsc { get; set; }
    }
}
