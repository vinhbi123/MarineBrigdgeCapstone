using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Orders;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Orders.Query.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetAllOrderQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
        private readonly IClaimService _claimService;

        public GetOrderByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public async ValueTask<ApiResponse> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            var accountId = _claimService.GetCurrentUserId;
            var role = Enum.Parse<ERole>(_claimService.GetRole);
            var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: o => o.Id == request.Id,
                include: o => o.Include(o => o.Boatyard)
                    .ThenInclude(b => b.Account)
                    .Include(o => o.Ship)
                    .ThenInclude(s => s.Captain)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                    .ThenInclude(p => p.Category)
                    .ThenInclude(c => c.Supplier)) ?? throw new NotFoundException("Không tìm thấy đơn hàng");
            if (role == ERole.User)
            {
                if (order.Ship.AccountId != accountId)
                {
                    throw new BadHttpRequestException("Đơn hàng không phải của khách hàng này");
                }
            }
            else if (role == ERole.Boatyard)
            {
                if (order.Boatyard.AccountId != accountId)
                {
                    throw new BadHttpRequestException("Đơn hàng không phải của xưởng này");
                }
            }

            var response = new GetOrderResponse
            {
                Id = order.Id,
                ShipId = order.ShipId,
                ShipName = order.Ship?.Name,
                BoatyardId = order.BoatyardId,
                Longitude = order.Boatyard != null ? order.Boatyard.Longitude : order.Ship?.Longitude,
                Latitude = order.Boatyard != null ? order.Boatyard.Latitude : order.Ship?.Latitude,
                BoatyardName = order.Boatyard?.Name,
                Phone = order.BoatyardId != null ? order.Boatyard?.Account.PhoneNumber : order.Ship?.Captain.PhoneNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new GetOrderItemsResponse
                {
                    Id = oi.Id,
                    SupplierId = oi.ProductVariant.Product.SupplierId,
                    SupplierName = oi.ProductVariant.Product.Category.Supplier.Name,
                    ProductVariantId = oi.ProductVariantId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    ProductOptionName = oi.ProductOptionName,
                    ProductVariantName = oi.ProductVariant.Name
                }).ToList()
            };
            return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy chi tiết đơn hàng thành công",
                Data = response
            };
        }
    }
}