using Mediator;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Booking;
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
                selector: b => new GetAllBookingResponse()
                {
                    Id = b.Id,
                    ShipId = b.ShipId,
                    ShipName = b.Ship.Name,
                    ShipOwnerName = b.Ship.Account.FullName,
                    ShipOwnerPhoneNumber = b.Ship.Account.PhoneNumber,
                    DockSlotId = b.DockSlotId,
                    DockSlotName = b.DockSlot.Name,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Type = b.Type,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status
                },
                predicate: b => (role != ERole.Boatyard || b.Status != EBookingStatus.Pending) &&
                                (role != ERole.User || b.Ship.AccountId == userId) &&
                                (request.StartDate == null || DateOnly.FromDateTime(b.CreatedDate) >= request.StartDate) &&
                                (request.EndDate == null || DateOnly.FromDateTime(b.CreatedDate) <= request.EndDate),
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