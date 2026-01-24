using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Orders;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Application.Features.Orders.Query.GetOrder
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
        private readonly IClaimService _claimService;
        public GetAllOrdersQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public async ValueTask<ApiResponse> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var role = Enum.Parse<ERole>(_claimService.GetRole);
            var userId = _claimService.GetCurrentUserId;
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id == userId,
                include: a => a.Include(a => a.Boatyard)
                    .Include(a => a.Supplier)) ?? throw new NotFoundException("Không tìm thấy người dùng");
            var orders = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: o => new GetAllOrderResponse
                {
                    Id = o.Id,
                    ShipId = o.ShipId,
                    ShipName = o.Ship.Name,
                    BoatyardId = o.BoatyardId,
                    BoatyardName = o.Boatyard.Name,
                    Phone = o.BoatyardId != null ? o.Boatyard.Account.PhoneNumber : o.Ship.Captain.PhoneNumber,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status
                },
                predicate: o =>
                    (role != ERole.Supplier || (o.Status != EOrderStatus.Pending && 
                                                o.OrderItems.Any(oi => oi.ProductVariant.Product.SupplierId == account.Supplier.Id))) &&
                    (role != ERole.Boatyard || o.BoatyardId == account.Boatyard.Id) &&
                    (role != ERole.User || o.Ship.AccountId == userId) &&
                    (request.SupplierId == null || o.OrderItems.Any(oi => oi.ProductVariant.Product.Category.SupplierId == request.SupplierId)) &&
                    (request.StartDate == null || DateOnly.FromDateTime(o.CreatedDate) >= request.StartDate) &&
                    (request.EndDate == null || DateOnly.FromDateTime(o.CreatedDate) <= request.EndDate) &&
                    (!request.ShipId.HasValue || o.ShipId == request.ShipId) &&
                    (string.IsNullOrEmpty(request.Status) || o.Status.ToString() == request.Status) &&
                    (string.IsNullOrEmpty(request.Search) || o.Id.ToString().Contains(request.Search)),
                include: o => o.Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                    .ThenInclude(pv => pv.Product),
                page: request.Page,
                size: request.PageSize,
                sortBy: request.SortBy ?? nameof(Order.CreatedDate),
                isAsc: request.IsAsc
            );

            return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy danh sách đơn hàng thành công",
                Data = orders
            };
        }
    }
}
