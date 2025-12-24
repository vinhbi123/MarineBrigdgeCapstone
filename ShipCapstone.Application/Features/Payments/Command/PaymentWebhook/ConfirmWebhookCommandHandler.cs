using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using ShipCapstone.Infrastructure.Utils;


namespace ShipCapstone.Application.Features.Payments.Command.PaymentWebhook;

public class ConfirmWebhookCommandHandler : IRequestHandler<ConfirmWebhookCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

    public ConfirmWebhookCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async ValueTask<ApiResponse> Handle(ConfirmWebhookCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        var code = payload.code;
        var successPayment = payload.success;

        var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
            predicate: t => t.TransactionCode == payload.data.orderCode.ToString()) ?? throw new NotFoundException("Không tìm thấy giao dịch");

        if (successPayment && code == "00")
        {
            transaction.LastModifiedDate = TimeUtil.GetCurrentSEATime();
            transaction.Status = ETransactionStatus.Approved;
            _unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);

            if (transaction.Type == EPaymentType.Supplier)
            {
                var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                    predicate: o => o.Id == transaction.OrderId);

                order.Status = EOrderStatus.Approved;
                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                
            }
            else if (transaction.Type == EPaymentType.Boatyard)
            {
                var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                    predicate: b => b.Id == transaction.BookingId) ?? throw new NotFoundException("Không tìm thấy đặt lịch");

                booking.Status = EBookingStatus.Confirmed;
                booking.LastModifiedDate = TimeUtil.GetCurrentSEATime();
                _unitOfWork.GetRepository<Booking>().UpdateAsync(booking);
            }

            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK,
                Message = "Xử lý thanh toán thành công",
                Data = transaction.Id
            };
        }

        throw new Exception("Xử lý thanh toán thất bại");
    }
}