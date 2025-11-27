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

namespace ShipCapstone.Application.Features.Revenues.Command.CreateUrlPaymentRevenue;

public class CreateUrlPaymentRevenueCommandHandler : IRequestHandler<CreateUrlPaymentRevenueCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IPaymentService _paymentService;
    public CreateUrlPaymentRevenueCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IPaymentService paymentService)
    {
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async ValueTask<ApiResponse> Handle(CreateUrlPaymentRevenueCommand request, CancellationToken cancellationToken)
    {
        var url = "";
        Transaction transaction;
        string referenceCode;
        if (request.Type.Equals(ERevenueType.Supplier))
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.Id)) ?? throw new NotFoundException("Không tìm thấy thông tin đại lí");

            var orders = await _unitOfWork.GetRepository<Order>().GetListAsync(
                predicate: o => o.OrderItems.Any(oi => oi.ProductVariant.Product.SupplierId == supplier.Id)
                                && o.CreatedDate >= request.StartDate.ToDateTime(TimeOnly.MinValue)
                                && o.CreatedDate <= request.EndDate.ToDateTime(TimeOnly.MaxValue)
                                && !(o.Status.Equals(EOrderStatus.Pending) || o.Status.Equals(EOrderStatus.Rejected)));
            var totalAmountOrder = orders.Sum(o => o.TotalAmount);
            var revenue = totalAmountOrder;
            var revenueNumber = Math.Round(revenue, 0);
            referenceCode = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmss}-{supplier.Id.ToString().Substring(0, 6)}";
            string description = $"{referenceCode} - TT doanh thu {supplier.Name} tu {request.StartDate} den {request.EndDate}";
            CreatePaymentSePayRequest paymentSePayRequest = new CreatePaymentSePayRequest()
            {
                BankName = supplier.BankName,
                BankNo = supplier.BankNo,
                Revenue = revenueNumber,
                Description = description
            };
            url = _paymentService.CreateUrlSepay(paymentSePayRequest);
            transaction = new Transaction()
            {
                Id = Guid.CreateVersion7(),
                Amount = revenue,
                CreatedDate = TimeUtil.GetCurrentSEATime(),
                Status = ETransactionStatus.Pending,
                TransactionCode = referenceCode,
                Type = EPaymentType.Revenue
            };
        }
        else
        {
            var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
                predicate: g => g.Id.Equals(request.Id)) ?? throw new NotFoundException("Không tìm thấy thông tin xưởng");

            var bookings = await _unitOfWork.GetRepository<Booking>().GetListAsync(
                predicate: b => b.BookingServices.Any(bs => bs.BoatyardService.BoatyardId == boatyard.Id)
                                && b.CreatedDate >= request.StartDate.ToDateTime(TimeOnly.MinValue)
                                && b.CreatedDate <= request.EndDate.ToDateTime(TimeOnly.MaxValue)
                                && !(b.Status.Equals(EBookingStatus.Pending) || b.Status.Equals(EBookingStatus.Cancelled)),
                include: b => b.Include(b => b.BookingServices)
                    .ThenInclude(bs => bs.BoatyardService));
            var totalAmountService = bookings.Sum(b => b.BookingServices.Select(bs => bs.BoatyardService.Price).Sum(p => p));
            var revenue = totalAmountService;
            var revenueNumber = Math.Round(revenue, 0);
            referenceCode = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmss}-{boatyard.Id.ToString().Substring(0, 6)}";
            string description = $"{referenceCode} - TT doanh thu {boatyard.Name} tu {request.StartDate} den {request.EndDate}";
            CreatePaymentSePayRequest paymentSePayRequest = new CreatePaymentSePayRequest()
            {
                BankName = boatyard.BankName,
                BankNo = boatyard.BankNo,
                Revenue = revenueNumber,
                Description = description
            };
            url = _paymentService.CreateUrlSepay(paymentSePayRequest);
            transaction = new Transaction()
            {
                Id = Guid.CreateVersion7(),
                Amount = revenue,
                CreatedDate = TimeUtil.GetCurrentSEATime(),
                Status = ETransactionStatus.Pending,
                TransactionCode = referenceCode,
                Type = EPaymentType.Revenue
            };
        }

        await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
        bool isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra trong quá trình tạo mã thanh toán doanh số");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Tạo đơn doanh thu thành công",
            Data = url
        };
    }
}