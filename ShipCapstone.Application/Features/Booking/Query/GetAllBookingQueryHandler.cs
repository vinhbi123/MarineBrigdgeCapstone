//using Mediator;
//using Microsoft.EntityFrameworkCore;
//using ShipCapstone.Domain.Entities;
//using ShipCapstone.Domain.Models.Booking;
//using ShipCapstone.Domain.Models.Common;
//using ShipCapstone.Infrastructure.Persistence;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using BookingEntity = ShipCapstone.Domain.Entities.Booking;

//namespace ShipCapstone.Application.Features.Bookings.Query.GetAllBooking
//{
//    public class GetAllBookingQueryHandler : IRequestHandler<GetAllBookingQuery, ApiResponse>
//    {
//        private readonly ShipCapstoneContext _context;

//        public GetAllBookingQueryHandler(ShipCapstoneContext context)
//        {
//            _context = context;
//        }

//        public async ValueTask<ApiResponse> Handle(GetAllBookingQuery request, CancellationToken cancellationToken)
//        {
//            var bookings = await _context.Bookings
//                .Include(b => b.BookingServices)
//                .ToListAsync(cancellationToken);

//            var response = bookings.Select(b => new GetAllBookingResponse
//            {
//                Id = b.Id,
//                ShipId = b.ShipId,
//                DockSlotId = b.DockSlotId,
//                StartTime = b.StartTime,
//                EndTime = b.EndTime,
//                Type = b.Type,
//                TotalAmount = b.TotalAmount,
//                Status = b.Status,
//                Services = b.BookingServices?.Select(bs => bs.BoatyardServiceId).ToList() ?? new List<Guid>()
//            }).ToList();

//            return new ApiResponse
//            {
//                Status = 200,
//                Message = "Lấy danh sách booking thành công",
//                Data = response
//            };
//        }
//    }
//}
