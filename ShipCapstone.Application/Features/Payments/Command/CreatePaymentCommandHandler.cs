using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Payments;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using ShipCapstone.Infrastructure.Utils;

namespace ShipCapstone.Application.Features.Payments.Command;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, ApiResponse>
{
    private readonly IClaimService _claimService;
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IPaymentService _paymentService;

    public CreatePaymentCommandHandler(IClaimService claimService, IUnitOfWork<ShipCapstoneContext> unitOfWork, IPaymentService paymentService)
    {
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    }

    public async ValueTask<ApiResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId)) ?? throw new NotFoundException("Không tìm thấy thông tin tài khoản");
        object? paymentObject = null;
        if (request.Type == EPaymentType.Supplier)
        {
            var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: o => o.Id.Equals(request.Id),
               include: o => o.Include(o => o.Ship)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)) ?? throw new NotFoundException("Không tìm thấy đơn hàng");
            if (order.Ship.AccountId != account.Id)   
            {
                throw new BadHttpRequestException("Đơn hàng không phải của tài khoản này");
            }
            paymentObject = order;
        }

        else if (request.Type == EPaymentType.Boatyard)
        {
            var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: b => b.Id.Equals(request.Id),
                include: b => b.Include(b => b.Ship)) ?? throw new NotFoundException("Không tìm thấy thông tin đặt dịch vụ");

            if (booking.Ship.AccountId != account.Id)
            {
                throw new BadHttpRequestException("Dịch vụ này không thuộc tài khoản này");
            }

            paymentObject = booking;
        }
        else
        {
            throw new BadHttpRequestException("Kiểu thanh toán không hợp lệ");
        }
            Random random = new Random();
        var transactionCode = DateTime.Now.Ticks % 10000000000000L * 10 + random.Next(0, 10);   

        CreatePaymentRequest paymentRequest = new CreatePaymentRequest()
        {
            Account = account,
            PaymentObject = paymentObject,
            Address = request.Address,
            Type = request.Type,
            TransactionCode = transactionCode
        };

        var url = await _paymentService.CreatePaymentUrl(paymentRequest);
        var transaction = new Transaction()
        {
            Id = Guid.CreateVersion7(),
            Status = ETransactionStatus.Pending,
            CreatedDate = TimeUtil.GetCurrentSEATime(),
            TransactionCode = transactionCode.ToString()
        };
        if (request.Type == EPaymentType.Supplier)
        {
            var order = (Order)paymentObject;
            transaction.OrderId = order.Id;
            transaction.Amount = order.TotalAmount;
        }
        else if (request.Type == EPaymentType.Boatyard)
        {
            var booking = (Booking)paymentObject;
            transaction.BookingId = booking.Id;
            transaction.Amount = booking.TotalAmount;
        }
        await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
        bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Lỗi khi xảy ra tạo đơn thanh toán");
        }
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Tạo thanh toán thành công",
            Data = url
        };
    }
}