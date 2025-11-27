using Mediator;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Bookings.Query.GetBooking
{
    public class GetAllBookingQueryHandler : IRequestHandler<GetAllBookingQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
        private readonly IClaimService _claimService;
        public GetAllBookingQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }
        public async ValueTask<ApiResponse> Handle(GetAllBookingQuery request, CancellationToken cancellationToken)
        {
            var role = Enum.Parse<ERole>(_claimService.GetRole);
            var userId = _claimService.GetCurrentUserId;
            var bookings = await _unitOfWork.GetRepository<Booking>().GetPagingListAsync(
                predicate: b => (role != ERole.Boatyard || b.Status != EBookingStatus.Pending) &&
                                (role != ERole.User || b.Ship.AccountId == userId),
                page: request.Page,
                size: request.Size,
                sortBy: request.SortBy ?? nameof(Booking.CreatedDate),
                isAsc: request.IsAsc);

            return new ApiResponse
            {
                Status = 200,
                Message = "Lấy danh sách booking thành công",
                Data = bookings
            };
        }
    }
}