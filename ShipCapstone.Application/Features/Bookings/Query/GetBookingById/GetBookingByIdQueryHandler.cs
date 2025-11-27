using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Booking;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Bookings.Query.GetBookingById;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, ApiResponse>
{
    private readonly IClaimService _claimService;
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

    public GetBookingByIdQueryHandler(IClaimService claimService, IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async ValueTask<ApiResponse> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var role = Enum.Parse<ERole>(_claimService.GetRole);
        var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
            predicate: b => b.Id == request.Id,
            include: b => b.Include(b => b.Ship)
                .Include(b => b.BookingServices)
                .ThenInclude(bs => bs.BoatyardService)
                .ThenInclude(bs => bs.Boatyard)) ?? throw new NotFoundException("Không tìm thấy đơn đặt dịch vụ");
        if (role == ERole.User)
        {
            if (booking.Ship.AccountId != accountId)
            {
                throw new BadHttpRequestException("Đơn đặt dịch vụ này không phải của bạn");
            }
        }
        if (role == ERole.Boatyard)
        {
            var isOwner = booking.BookingServices
                .Any(bs => bs.BoatyardService.Boatyard.AccountId == accountId);

            if (!isOwner)
            {
                throw new BadHttpRequestException("Đơn đặt này không phải của xưởng bạn");
            }
        }

        var response = new GetBookingByIdResponse()
        {
            Id = booking.Id,
            Status = booking.Status,
            TotalAmount = booking.TotalAmount,
            Type = booking.Type,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            ShipId = booking.ShipId,
            DockSlotId = booking.DockSlotId
        };

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy đơn đặt dịch vụ thành công",
            Data = response
        };
    }
}