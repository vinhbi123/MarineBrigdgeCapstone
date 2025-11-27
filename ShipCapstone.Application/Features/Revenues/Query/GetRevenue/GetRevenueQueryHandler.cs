using Mediator;
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
                                && (o.Status != EOrderStatus.Pending || o.Status != EOrderStatus.Rejected)
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
                    NetRevenue = g.Sum(o => o.TotalAmount) - g.Sum(o => o.TotalAmount)
                })
                .Where(o => o.TotalRevenue > 0)
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToList();
            foreach (var group in groupRevenue)
            {
                revenues.Add(new GetRevenueResponse
                {
                    Month = group.Month.ToString("D2"),
                    Year = group.Year.ToString(),
                    TotalRevenue = group.TotalRevenue,
                    NetRevenue = group.NetRevenue,
                });

            }
        }
        else if (role == ERole.Boatyard)
        {
            var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
                predicate: s => s.AccountId == accountId) ?? throw new NotFoundException("Không tìm thấy nhà cung cấp");
            var orders = await _unitOfWork.GetRepository<Booking>().GetListAsync(
                predicate: b => b.BookingServices.Any(bs => bs.BoatyardService.BoatyardId == boatyard.Id)
                                && (b.Status == EBookingStatus.Pending)
                                && b.CreatedDate >= startDateQuery
                                && b.CreatedDate <= endDateQuery);
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
                    NetRevenue = g.Sum(o => o.TotalAmount) - g.Sum(o => o.TotalAmount)
                })
                .Where(o => o.TotalRevenue > 0)
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToList();
            foreach (var group in groupRevenue)
            {
                revenues.Add(new GetRevenueResponse
                {
                    Month = group.Month.ToString("D2"),
                    Year = group.Year.ToString(),
                    TotalRevenue = group.TotalRevenue,
                    NetRevenue = group.NetRevenue,
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