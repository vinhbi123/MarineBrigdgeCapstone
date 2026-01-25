using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Revenues;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Revenues.Query.GetRevenue;

public class GetRevenueQueryHandler : IRequestHandler<GetRevenueQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public GetRevenueQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    public async ValueTask<ApiResponse> Handle(GetRevenueQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var role = Enum.Parse<ERole>(_claimService.GetRole);
        var revenues = new List<GetRevenueResponse>();
        var startDateQuery = request.StartDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue;
        var endDateQuery = request.EndDate?.ToDateTime(TimeOnly.MaxValue) ?? DateTime.MaxValue;
        if (role == ERole.Supplier)
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
                predicate: s => s.AccountId == accountId) ?? throw new NotFoundException("Không tìm thấy nhà cung cấp");
            var orders = await _unitOfWork.GetRepository<Order>().GetListAsync(
                predicate: o => o.OrderItems.Any(oi => oi.ProductVariant.Product.SupplierId == supplier.Id)
                                && o.Status != EOrderStatus.Pending && o.Status != EOrderStatus.Rejected
                                && o.CreatedDate >= startDateQuery
                                && o.CreatedDate <= endDateQuery);
            var groupRevenue = orders
                .GroupBy(o => new
                {
                    o.CreatedDate.Month,
                    o.CreatedDate.Year
                })
                .Select(g => new
                {
                    g.Key.Month,
                    g.Key.Year,
                    TotalRevenue = g.Sum(o => o.TotalAmount),
                    NetRevenue = g.Sum(o => o.TotalAmount) - g.Sum(o => o.TotalAmount * supplier.CommissionFeePercent / 100m)
                })
                .Where(o => o.TotalRevenue > 0)
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToList();
            foreach (var group in groupRevenue)
            {
                var startDate = new DateTime(group.Year, group.Month, 1);
                var endDate = startDate.AddMonths(1).AddTicks(-1);
                
                var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                    predicate: t => t.SupplierId.Equals(supplier.Id)
                                    && t.Type == EPaymentType.Revenue
                                    && t.CreatedDate >= startDate
                                    && t.CreatedDate < endDate
                                    && t.Status == ETransactionStatus.Approved);
                revenues.Add(new GetRevenueResponse
                {
                    Month = group.Month.ToString("D2"),
                    Year = group.Year.ToString(),
                    TotalRevenue = group.TotalRevenue,
                    NetRevenue = group.NetRevenue,
                    IsTransferred = transaction != null,
                    TransferredDate = transaction?.CreatedDate,
                });
                
            }
        }
        else if (role == ERole.Boatyard)
        {
            var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
                predicate: s => s.AccountId == accountId) ?? throw new NotFoundException("Không tìm thấy nhà cung cấp");
            var bookings = await _unitOfWork.GetRepository<Booking>().GetListAsync(
                predicate: b => b.BookingServices.Any(bs => bs.BoatyardService.BoatyardId == boatyard.Id)
                                && b.Status != EBookingStatus.Cancelled 
                                && b.Status != EBookingStatus.Pending
                                && b.CreatedDate >= startDateQuery
                                && b.CreatedDate <= endDateQuery);
            var groupRevenue = bookings
                .GroupBy(o => new
                {
                    o.CreatedDate.Month,
                    o.CreatedDate.Year
                })
                .Select(g => new
                {
                    g.Key.Month,
                    g.Key.Year,
                    TotalRevenue = g.Sum(o => o.TotalAmount),
                    NetRevenue = g.Sum(o => o.TotalAmount) - g.Sum(o => o.TotalAmount * boatyard.CommissionFeePercent / 100m)
                })
                .Where(o => o.TotalRevenue > 0)
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToList();
            foreach (var group in groupRevenue)
            {
                var startDate = new DateTime(group.Year, group.Month, 1);
                var endDate = startDate.AddMonths(1).AddTicks(-1);
                
                var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                    predicate: t => t.BoatyardId.Equals(boatyard.Id)
                                    && t.Type == EPaymentType.Revenue
                                    && t.CreatedDate >= startDate
                                    && t.CreatedDate < endDate
                                    && t.Status == ETransactionStatus.Approved);
                revenues.Add(new GetRevenueResponse
                {
                    Month = group.Month.ToString("D2"),
                    Year = group.Year.ToString(),
                    TotalRevenue = group.TotalRevenue,
                    NetRevenue = group.NetRevenue,
                    IsTransferred = transaction != null,
                    TransferredDate = transaction?.CreatedDate,
                });
                
            }
        }
        else if (role == ERole.Admin)
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetListAsync(
                predicate: o => o.Status != EOrderStatus.Pending 
                                && o.Status != EOrderStatus.Rejected 
                                && o.CreatedDate >= startDateQuery
                                && o.CreatedDate <= endDateQuery,
                include: o => o.Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                    .ThenInclude(p => p.Category)
                    .ThenInclude(c => c.Supplier));

            var orderRevenue = orders
                .GroupBy(o => new
                {
                    o.CreatedDate.Month,
                    o.CreatedDate.Year
                })
                .Select(g => new
                {
                    g.Key.Month,
                    g.Key.Year,
                    TotalRevenue = g.Sum(o => o.TotalAmount),
                    NetRevenue = g.Sum(o => o.OrderItems.
                        Sum(oi => oi.Price * oi.Quantity * (oi.ProductVariant.Product.Category.Supplier.CommissionFeePercent / 100m)))
                })
                .Where(og => og.TotalRevenue > 0)
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToList();

            var serviceAppointments = await _unitOfWork.GetRepository<Booking>().GetListAsync(
                predicate: sa => sa.Status == EBookingStatus.Confirmed
                                 && sa.CreatedDate >= startDateQuery
                                 && sa.CreatedDate <= endDateQuery,
                include: sa => sa.Include(sa => sa.BookingServices).ThenInclude(bs => bs.BoatyardService)
                    .ThenInclude(bs => bs.Boatyard));
                
            var serviceAppointmentRevenue = serviceAppointments
                .GroupBy(sa => new
                {
                    sa.CreatedDate.Month,
                    sa.CreatedDate.Year
                })
                .Select(g => new
                {
                    g.Key.Month,
                    g.Key.Year,
                    TotalRevenue = g.Sum(b => b.TotalAmount),
                    NetRevenue = g.Sum(b => b.BookingServices.Sum(bs => bs.BoatyardService.Price * (bs.BoatyardService.Boatyard.CommissionFeePercent / 100m)))
                })
                .Where(o => o.TotalRevenue > 0)
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToList();
            
            var allRevenue = orderRevenue.Concat(serviceAppointmentRevenue)
                .GroupBy(r => new { r.Month, r.Year })
                .Select(g => new
                {
                    g.Key.Month,
                    g.Key.Year,
                    TotalRevenue = g.Sum(x => x.TotalRevenue),
                    NetRevenue = g.Sum(x => x.NetRevenue)
                })
                .OrderByDescending(g => g.Year)
                .ThenByDescending(g => g.Month)
                .ToList();
            
            foreach (var revenue in allRevenue)
            {
                revenues.Add(new GetRevenueResponse
                {
                    Month = revenue.Month.ToString("D2"),
                    Year = revenue.Year.ToString(),
                    TotalRevenue = revenue.TotalRevenue,
                    NetRevenue = revenue.NetRevenue,
                    IsTransferred = false,
                    TransferredDate = null,
                });
            }
        }

        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy lợi nhuận thành công",
            Data = revenues
        };
    }
}