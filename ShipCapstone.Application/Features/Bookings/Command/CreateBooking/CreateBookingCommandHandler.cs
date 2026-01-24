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

using BookingEntity = ShipCapstone.Domain.Entities.Booking;
namespace ShipCapstone.Application.Features.Bookings.Command.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ApiResponse>
    {
        private readonly IClaimService _claimService;
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

        public CreateBookingCommandHandler(IClaimService claimService, IUnitOfWork<ShipCapstoneContext> unitOfWork)
        {
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async ValueTask<ApiResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var accountId = _claimService.GetCurrentUserId;
            if (accountId == Guid.Empty)
                throw new BadHttpRequestException("Không tìm thấy tài khoản.");

            var account = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(
                    predicate: a => a.Id == accountId,
                    include: a => a.Include(a => a.Ships))
                ?? throw new NotFoundException("Không tìm thấy tài khoản.");

            var ship = await _unitOfWork.GetRepository<Ship>()
                .SingleOrDefaultAsync(predicate: s => s.Id == request.ShipId)
                ?? throw new NotFoundException("Không tìm thấy tàu");

            if (ship.AccountId != accountId)
                throw new BadHttpRequestException("Tàu này không thuộc về bạn.");

            var dockSlot = await _unitOfWork.GetRepository<DockSlot>()
                .SingleOrDefaultAsync(predicate: d => d.Id == request.DockSlotId && d.Bookings.All(b => b.Status != EBookingStatus.Pending))
                ?? throw new NotFoundException("Không tìm thấy DockSlot");

            var booking = new BookingEntity
            {
                Id = Guid.NewGuid(),
                ShipId = ship.Id,
                DockSlotId = dockSlot.Id,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Type = request.Type,
                Status = EBookingStatus.Pending
            };

            await _unitOfWork.GetRepository<BookingEntity>().InsertAsync(booking);

            decimal totalAmount = 0;
            Guid? boatyaltId = null;

            if (request.Services.Any())
            {
                foreach (var serviceId in request.Services)
                {
                    var service = await _unitOfWork.GetRepository<BoatyardService>()
                        .SingleOrDefaultAsync(predicate: s => s.Id == serviceId && s.IsActive)
                        ?? throw new NotFoundException($"Service {serviceId} không tồn tại.");
                    if (boatyaltId == null)
                    {
                        boatyaltId = service.BoatyardId;
                    }
                    else if (boatyaltId != service.BoatyardId)
                    {
                        throw new BadHttpRequestException("Chỉ được đặt cùng 1 xưởng");
                    }

                    totalAmount += service.Price;

                    booking.BookingServices.Add(new BookingService
                    {
                        Id = Guid.NewGuid(),
                        BookingId = booking.Id,
                        BoatyardServiceId = service.Id
                    });
                }
            }

            booking.TotalAmount = totalAmount;

            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
                throw new Exception("Có lỗi khi tạo booking.");

            var response = new CreateBookingResponse
            {
                Id = booking.Id,
                ShipId = booking.ShipId,
                DockSlotId = booking.DockSlotId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Type = booking.Type,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                Services = request.Services
            };

            return new ApiResponse
            {
                Status = 200,
                Message = "Tạo booking thành công",
                Data = response
            };
        }
    }
}
