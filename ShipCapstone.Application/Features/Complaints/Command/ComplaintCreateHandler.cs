using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Complaints.Command.CreateComplaint;

public class CreateComplaintCommandHandler : IRequestHandler<CreateComplaintCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;
    private readonly ILogger _logger;

    public CreateComplaintCommandHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork,
        IClaimService claimService,
        ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _claimService = claimService;
        _logger = logger;
    }

    public async ValueTask<ApiResponse> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _claimService.GetCurrentUserId;

        if (currentUserId == Guid.Empty)
            throw new BadHttpRequestException("Tài khoản không hợp lệ");

        var receiver = await _unitOfWork.GetRepository<Account>()
            .SingleOrDefaultAsync(x => x.Id == request.ReceiverAccountId);

        if (receiver == null)
            throw new NotFoundException("Không tìm thấy người nhận complaint");

        if (request.OrderId != null)
        {
            var order = await _unitOfWork.GetRepository<Order>()
                .SingleOrDefaultAsync(o => o.Id == request.OrderId);

            if (order == null)
                throw new NotFoundException("Order không tồn tại");
        }

        if (request.BookingId != null)
        {
            var booking = await _unitOfWork.GetRepository<Booking>()
                .SingleOrDefaultAsync(o => o.Id == request.BookingId);

            if (booking == null)
                throw new NotFoundException("Booking không tồn tại");
        }

        var complaint = new Complaint
        {
            Content = request.Content,
            Status = EComplaintStatus.Pending,
            AccountId = currentUserId, 
            OrderId = request.OrderId,
            BookingId = request.BookingId,
        };

        await _unitOfWork.GetRepository<Complaint>().InsertAsync(complaint);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
            throw new Exception("Tạo complaint thất bại");

        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Gửi complaint thành công",
            Data = complaint.Id
        };
    }
}
